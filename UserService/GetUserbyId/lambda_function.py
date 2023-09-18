import boto3
import json

def lambda_handler(event, context):
    client = boto3.client('cognito-idp')
    user_id = None
    if event.get('pathParameters'):
        user_id = event['pathParameters'].get('userId')
    if not user_id and event.get('queryStringParameters'):
        user_id = event['queryStringParameters'].get('userId')

    try:
        response = client.admin_get_user(
            UserPoolId='us-east-1_YwI4ucIqm',
            Username=user_id
        )

        user_attributes = response['UserAttributes']
        email = next((item for item in user_attributes if item["Name"] == "email"), None)
        sub = next((item for item in user_attributes if item["Name"] == "sub"), None)

        return {
            'statusCode': 200,
            'body': json.dumps({
         'email': email['Value'] if email else None,
                'sid': sub['Value'] if sub else None
            })
        }

    except client.exceptions.UserNotFoundException:
        return {
            'statusCode': 404,
            'body': json.dumps({'error': 'User not found'})
        }

    except Exception as e:
        return {
            'statusCode': 500,
            'body': json.dumps({'error': 'An error occurred', 'message': str(e)})
        }
