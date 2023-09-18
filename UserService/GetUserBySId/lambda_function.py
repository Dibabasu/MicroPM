import boto3
import json

def lambda_handler(event, context):
    client = boto3.client('cognito-idp')
    sub = None
    if event.get('pathParameters'):
        sub = event['pathParameters'].get('sub')
    if not sub and event.get('queryStringParameters'):
        sub = event['queryStringParameters'].get('sub')

    try:
        response = client.list_users(
            UserPoolId='us-east-1_YwI4ucIqm',
            Filter='sub = "{}"'.format(sub),       
            Limit=1
        )

        if not response['Users']:
            return {
                'statusCode': 404,
                'body': json.dumps({'error': 'User not found'})
            }

        user = response['Users'][0]
    

        return {
            'statusCode': 200,
            'body': json.dumps({
          'userId': user['Username']
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
