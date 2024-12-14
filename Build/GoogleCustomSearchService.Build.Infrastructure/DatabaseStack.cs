using System.Globalization;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.CustomResources;
using Constructs;

namespace GoogleCustomSearchService.Build.Infrastructure;

public class DatabaseStackProps : StackProps 
{
    public Function MigrationLambda { get; set; }
}

public class DatabaseStack : Stack
{
    public static Vpc Vpc;

    public DatabaseStack(Construct scope, string id, DatabaseStackProps props) : base(scope, id)
    {
        Vpc vpc = new Vpc(this, "postgres-vpc", new VpcProps
        {
            Cidr = "10.0.0.0/16",
            MaxAzs = 2,
            SubnetConfiguration = new []
            {
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PUBLIC,
                    Name = "PostgresDbPublicSubnet"
                }
            }
        });

        SecurityGroup securityGroup = new SecurityGroup(this, "db-security-group", new SecurityGroupProps
        {
            Vpc = vpc
        });

        //ToDo: for now, confirm with command: aws ec2 describe-instances --region us-east-1 but when using a Load Balancer, implement a NAT to assign a range of ips to one ip and add this ip to the Ingress rule
        securityGroup.AddIngressRule(Peer.Ipv4("192.168.178.220/32"), Port.Tcp(5275), "Development PC");
        securityGroup.AddIngressRule(Peer.Ipv4("100.79.164.189/32"), Port.AllTcp(), "Personal Phone (4G)");
        securityGroup.AddIngressRule(Peer.Ipv4("184.73.72.154/32"), Port.AllTcp(), "UserServicePublicIp");
        securityGroup.AddIngressRule(Peer.Ipv4("172.31.44.182/32"), Port.AllTcp(), "UserServicePrivateIp");
        securityGroup.AddIngressRule(Peer.Ipv4("34.205.203.205/32"), Port.AllTcp(), "RecipeRandomizerPublicIp");
        securityGroup.AddIngressRule(Peer.Ipv4("172.31.4.22/32"), Port.AllTcp(), "RecipeRandomizerPrivateIp");


        const int dbPort = 5432;

        DatabaseInstance db = new DatabaseInstance(this, "recipe-randomizer-db", new DatabaseInstanceProps
        {
            SecurityGroups = new SecurityGroup[] 
            {
                securityGroup
            },
            Vpc = vpc,
            VpcSubnets = new SubnetSelection{ SubnetType = SubnetType.PUBLIC },
            Engine = DatabaseInstanceEngine.Postgres(new PostgresInstanceEngineProps
            {
                Version = PostgresEngineVersion.VER_16_3
            }),
            InstanceType = new Amazon.CDK.AWS.EC2.InstanceType("t3.micro"),  //t4g.micro or t3.micro for free tier - IMPORTANT
            AllocatedStorage = 5,
            Port = dbPort,
            DatabaseName = "Projects",
            InstanceIdentifier = "recipe-randomizer-db-instance",
            BackupRetention = Duration.Days(0), //Not a good idea for prod code, fine for dev
            DeleteAutomatedBackups = true
        });

        Vpc = vpc;

        /*Provider provider = new Provider(this, "database-migration-lambda-provider", new ProviderProps
        {
            OnEventHandler = props.MigrationLambda
        });*/
    }
}
