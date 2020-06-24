# IF ADDITIONAL RESOURCES ARE REQUIRED BY YOUR API, ADD THEM TO THIS FILE
#ENSURE THIS FILE IS PLACED WITHIN A 'terraform' FOLDER LOCATED AT THE ROOT PROJECT DIRECTORY

provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
    application_name = "resident-information-api"
    parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

# S3 BUCKET FOR THE STATE FILE - MAINTAINING THE STATE OF THE INFRASTRUCTURE YOU CREATE IS ESSENTIAL
terraform {
    backend "s3" {
        bucket  = "terraform-state-production-apis"
        encrypt = true
        region  = "eu-west-2"
        key     = "services/resident-information-api/state"
    }
}

data "aws_vpc" "production_vpc" {
    tags = {
        Name = "vpc-production-apis-production"
    }
}
data "aws_subnet_ids" "production" {
    vpc_id = data.aws_vpc.production_vpc.id
    filter {
        name   = "tag:Type"
        values = ["private"]
    }
}
data "aws_ssm_parameter" "resident_info_postgres_db_password" {
    name = "/resident-information-api/production/postgres-password"
}
data "aws_ssm_parameter" "resident_info_postgres_db_username" {
    name = "/resident-information-api/production/postgres-username"
}

module "postgres_db_production" {
    source = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/postgres"
    environment_name = "production"
    vpc_id = data.aws_vpc.production_vpc.id
    db_identifier = "resident-information"
    db_name = "resident_information"
    db_port  = 5200
    subnet_ids = data.aws_subnet_ids.production.ids
    db_engine = "postgres"
    db_engine_version = "11.1" //DMS does not work well with v12
    db_instance_class = "db.t2.micro"
    db_allocated_storage = 20
    maintenance_window ="sun:10:00-sun:10:30"
    db_username = data.aws_ssm_parameter.resident_info_postgres_db_username.value
    db_password = data.aws_ssm_parameter.resident_info_postgres_db_password.value
    storage_encrypted = false
    multi_az = true
    publicly_accessible = false
    project_name = "platform apis"
}

