
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticBeanstalk;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3.Assets;
using Constructs;

namespace GoogleCustomSearchService.Build.Infrastructure;

public class GoogleCustomSearchServiceElasticBeanstalkStackProps : StackProps
{
    public string ApplicationName { get; set; }
    public IVpc Vpc { get; set;}
}

public class GoogleCustomSearchServiceElasticBeanstalkStack : Stack
{
    public CfnApplication googleCustomSearchServiceElasticBeanstalkStack { get; set; }

    public GoogleCustomSearchServiceElasticBeanstalkStack(Construct scope, string id, GoogleCustomSearchServiceElasticBeanstalkStackProps props) : base(scope, id)
    {
        IRole role = Role.FromRoleName(this, "google-custom-search-service-eb-app-role", "aws-elasticbeanstalk-ec2-role");

        var instanceProfile = new InstanceProfile(this, "google-custom-search-service-eb-instance-profile", new InstanceProfileProps 
        {
            Role = role,
            InstanceProfileName = "GoogleCustomSearchServiceEBInstanceProfile"
        });

        _ = new CfnOutput(this, "google-custom-search-service-eb-iam-role", new CfnOutputProps 
        {
            Value = role.RoleArn
        });

        var archive = new Asset(this, "google-custom-search-service-app-zip-location", new AssetProps
        {
            Path = "../application.zip"
        });

        googleCustomSearchServiceElasticBeanstalkStack = new Amazon.CDK.AWS.ElasticBeanstalk.CfnApplication(this, "google-custom-search-service-elb-app", new CfnApplicationProps
        {
            ApplicationName = props.ApplicationName,
        });

        CfnApplicationVersion applicationVersion = new Amazon.CDK.AWS.ElasticBeanstalk.CfnApplicationVersion(this, "google-custom-search-service-elb-app-version", new CfnApplicationVersionProps
        {
            ApplicationName = props.ApplicationName,
            SourceBundle = new Amazon.CDK.AWS.ElasticBeanstalk.CfnApplicationVersion.SourceBundleProperty
            {
                S3Bucket = archive.S3BucketName,
                S3Key = archive.S3ObjectKey
            }
        });

        CfnEnvironment googleCustomSearchServiceElasticBeanstalkEnvironment = new CfnEnvironment(this, "google-custom-search-service-elb-environment", new CfnEnvironmentProps
        {
            ApplicationName = props.ApplicationName,
            OptionSettings = new CfnEnvironment.OptionSettingProperty[] 
            {
                new CfnEnvironment.OptionSettingProperty{ Namespace = "aws:autoscaling:launchconfiguration", OptionName = "IamInstanceProfile", Value = instanceProfile.InstanceProfileArn },
                new CfnEnvironment.OptionSettingProperty {Namespace = "aws:autoscaling:launchconfiguration", OptionName = "RootVolumeType", Value = "gp3"},
                new CfnEnvironment.OptionSettingProperty{ Namespace = "aws:autoscaling:asg", OptionName = "MaxSize", Value = "1" },
                new CfnEnvironment.OptionSettingProperty{ Namespace = "aws:autoscaling:asg", OptionName = "MinSize", Value = "1" },
                //new CfnEnvironment.OptionSettingProperty { Namespace = "aws:ec2:vpc", OptionName = "ELBSubnets", Value = ConcatenateVpcSubnetIds(props.Vpc.PublicSubnets) },
                //new CfnEnvironment.OptionSettingProperty { Namespace = "aws:ec2:vpc", OptionName = "Subnets", Value = ConcatenateVpcSubnetIds(props.Vpc.PrivateSubnets) },
                //new CfnEnvironment.OptionSettingProperty { Namespace = "aws:ec2:vpc", OptionName = "VPCId", Value = props.Vpc.VpcId }
            },
            EnvironmentName = "PROJECTS",   //Must be > 4 chars
            SolutionStackName = "64bit Amazon Linux 2023 v3.2.1 running .NET 8",
            VersionLabel = applicationVersion.Ref   //Critical apparently
        });

        googleCustomSearchServiceElasticBeanstalkEnvironment.AddDependency(googleCustomSearchServiceElasticBeanstalkStack);
        applicationVersion.AddDependency(googleCustomSearchServiceElasticBeanstalkStack);
    }

    /*private string ConcatenateVpcSubnetIds(IEnumerable<ISubnet> subnets)
    {
        if(!subnets.Any())
        {
            return string.Empty;
        }

        if(subnets.Count() == 1)
        {
            return subnets.Single().SubnetId;
        }

        StringBuilder sb = new StringBuilder();

        foreach(ISubnet subnet in subnets)
        {
            sb.Append(subnet.SubnetId);
            sb.Append(",");
        }

        return sb.ToString().TrimEnd(',');
    }*/
}
