# AWS API Gateway V2 to make requests to AWS Lambda
resource "aws_apigatewayv2_api" "websocket_api" {
  name                       = "golfgame_websocket_api"
  protocol_type              = "WEBSOCKET"
  route_selection_expression = "$request.body.action"
  tags                       = {
    golfgame = "1"
  }
}

# Dev stage for development. Other stages can be created for production, staging, etc.
resource "aws_apigatewayv2_stage" "production" {
  api_id      = aws_apigatewayv2_api.websocket_api.id
  name        = "production"
  auto_deploy = true

  default_route_settings {
    detailed_metrics_enabled = true
    data_trace_enabled = true
    logging_level = "INFO" // INFO, ERROR, or OFF
    throttling_burst_limit = 5000
    throttling_rate_limit = 10000
  }

  # What data to log to cloudwatch.
#  access_log_settings {
#    destination_arn = aws_cloudwatch_log_group.websocket_api_api_logs.arn
#
#    format = jsonencode({
#      requestId               = "$context.requestId"
#      sourceIp                = "$context.identity.sourceIp"
#      requestTime             = "$context.requestTime"
#      protocol                = "$context.protocol"
#      httpMethod              = "$context.httpMethod"
#      resourcePath            = "$context.resourcePath"
#      routeKey                = "$context.routeKey"
#      status                  = "$context.status"
#      responseLength          = "$context.responseLength"
#      integrationErrorMessage = "$context.integrationErrorMessage"
#    })
#  }
  tags = {
    golfgame = "1"
  }
}

# We create a log group for the lambda function to use.
resource "aws_cloudwatch_log_group" "websocket_api_api_logs" {
  name              = "/aws/api-gw/${aws_apigatewayv2_api.websocket_api.name}"
  retention_in_days = 14

  tags = {
    golfgame = "1"
  }
}

# Connect the lambda function to the API Gateway.
resource "aws_apigatewayv2_integration" "golf_lambda_integration" {
  api_id = aws_apigatewayv2_api.websocket_api.id

  integration_uri    = aws_lambda_function.golf_connect_lambda.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

resource "aws_apigatewayv2_route" "default" {
  api_id = aws_apigatewayv2_api.websocket_api.id
  route_key = "$default"
  target    = "integrations/${aws_apigatewayv2_integration.golf_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "connect" {
  api_id = aws_apigatewayv2_api.websocket_api.id
  route_key = "$connect"
  target    = "integrations/${aws_apigatewayv2_integration.golf_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "disconnect" {
  api_id = aws_apigatewayv2_api.websocket_api.id
  route_key = "$disconnect"
  target    = "integrations/${aws_apigatewayv2_integration.golf_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "notify" {
  api_id = aws_apigatewayv2_api.websocket_api.id
  route_key = "notify"
  target    = "integrations/${aws_apigatewayv2_integration.golf_lambda_integration.id}"
}

resource "aws_apigatewayv2_route" "host" {
  api_id = aws_apigatewayv2_api.websocket_api.id
  route_key = "host"
  target    = "integrations/${aws_apigatewayv2_integration.golf_lambda_integration.id}"
}

resource "aws_lambda_permission" "api_gateway_permission" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.golf_connect_lambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.websocket_api.execution_arn}/*/*"
}

output "production_base_url" {
  value = aws_apigatewayv2_stage.production.invoke_url
}
