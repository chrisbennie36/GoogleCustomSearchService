## Google Custom Search Engine Microservice

# .Net Core based Microservice which handles retrieving search results from a configurable Google Custom Search Engine based on an input query string

# Libraries used

    - MediatR
    - XUnit
    - AutoMapper
    - Serilog
    - NSwag

# Docker

Build image => docker build -f Dockerfile -t google-custom-search-service .
Run Container => docker run --rm -p <ConfiguredPortNumber>:8000 google-custom-search-service