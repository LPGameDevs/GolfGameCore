import {
    ApiGatewayManagementApiClient,
    PostToConnectionCommand
} from "@aws-sdk/client-apigatewaymanagementapi";

import {
    DynamoDBClient,
} from "@aws-sdk/client-dynamodb";

import {
    DynamoDBDocumentClient,
    PutCommand,
    ScanCommand,
    DeleteCommand,
} from "@aws-sdk/lib-dynamodb";


let apiClient;

const dynamoClient = new DynamoDBClient({});
const dynamoDBClient = DynamoDBDocumentClient.from(dynamoClient);

const tableName = "golf-active-connections";

export const handler = async (event, context) => {
        let body;
        let statusCode = 200;
        const headers = {
            "Content-Type": "application/json",
            // "Sec-WebSocket-Protocol" : "myprotocol"
        };

        const domain = event.requestContext.domainName;
        const stage = event.requestContext.stage;
        const connectionId = event.requestContext.connectionId;
        const callbackUrl = `https://${domain}/${stage}`;

        const apiConfig = {
            endpoint: callbackUrl,
            region: 'eu-north-1',
        };

        apiClient = new ApiGatewayManagementApiClient(apiConfig);

        try {
            let message;
            let connections;
            const route = event.requestContext.routeKey
            const incomingConnectionId = event.requestContext.connectionId

            switch (route) {
                case '$connect':
                    console.log('Connection occurred')
                    message = 'Hello from connect!'
                    await dynamoDBClient.send(
                        new PutCommand({
                            TableName: tableName,
                            Item: {
                                id: incomingConnectionId,
                            },
                        })
                    );
                    break
                case '$disconnect':
                    console.log('Disconnection occurred')
                    await dynamoDBClient.send(
                        new DeleteCommand({
                            TableName: tableName,
                            Key: {
                                id: incomingConnectionId,
                            },
                        })
                    );
                    message = 'Goodbye from disconnect!'
                    break

                case 'notify':
                    console.log('Received message:', event.requestContext.body);
                    const {person} = JSON.parse(event.body);

                    connections = await getConnections();

                    for (var i = 0; i < connections.length; i++) {
                        const connectionId = connections[i];
                        const messageResponse = await replyToMessage({message: "this is a message for " + person}, connectionId);
                        message = message + "---message sent to connection " + connectionId + " with " + messageResponse.$metadata.httpStatusCode + " status code.";
                    }
                    break

                case 'host':
                    console.log('Received message:', event.requestContext.body);
                    const {RequestType} = JSON.parse(event.body);

                    const serverCode = '54323';
                    let responseData = {
                        data: serverCode,
                        message: "new game hosted",
                        RequestType
                    }

                    connections = await getConnections();

                    for (var i = 0; i < connections.length; i++) {
                        const connectionId = connections[i];
                        const messageResponse = await replyToMessage(responseData, connectionId);
                        message = message + "---message sent to connection " + connectionId + " with " + messageResponse.$metadata.httpStatusCode + " status code.";
                    }
                    break

                default:
                    console.log('Received unknown route:', route)
            }


            body = {
                message,
                event: event.requestContext
            }
        } catch
            (err) {
            statusCode = 400;
            body = {
                message: err.message,
                event: event.requestContext
            };

        } finally {
            body = JSON.stringify(body);
        }

        return {
            statusCode,
            body,
            headers,
        };
    }
;

async function getConnections() {
    const response = await dynamoDBClient.send(
        new ScanCommand({
            TableName: tableName,
        })
    );

    if (response.Count == 0) {
        return [];
    }

    return response.Items.map((item) => item.id);
}

async function replyToMessage(response, connectionId) {
    const params = { // PostToConnectionRequest
        ConnectionId: connectionId,
        Data: Buffer.from(JSON.stringify(response))
    };
    const command = new PostToConnectionCommand(params);
    return apiClient.send(command);
};
