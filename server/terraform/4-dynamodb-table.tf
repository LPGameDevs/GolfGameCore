# We use dynamodb to store the level data of the game.

# Create the dynamodb table.
resource "aws_dynamodb_table" "golf-active-connections" {
  name                        = "golf-active-connections"
  hash_key                    = "id"
  billing_mode                = "PAY_PER_REQUEST" // As opposed to PROVISIONED
  deletion_protection_enabled = "false"
  stream_enabled = "false"
  table_class    = "STANDARD" // as opposed to STANDARD_INFREQUENT_ACCESS

  attribute {
    name = "id"
    type = "S"
  }

  point_in_time_recovery {
    enabled = "false"
  }

}

# IAM policy for the lambda to access the dynamodb table.
resource "aws_iam_policy" "dynamoDBLambdaPolicy" {
  name = "DynamoDBLambdaPolicy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "dynamodb:Scan",
          "dynamodb:Query",
          "dynamodb:DeleteItem",
          "dynamodb:GetItem",
          "dynamodb:PutItem"
        ]
        Resource = "*"
        #        Resource = [
        #          aws_dynamodb_table.golf-level-storage.arn
        #        ]
      }
    ]
  })
}

# Attach the policy to the lambda role.
resource "aws_iam_role_policy_attachment" "lambda-policy-attachment" {
  role       = aws_iam_role.golf_lambda_exec.name
  policy_arn = aws_iam_policy.dynamoDBLambdaPolicy.arn
}
