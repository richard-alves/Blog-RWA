# C# - Blog API
## API Information
- A simple Blog API in C# / ASP.NET Core, with two client applications that can optionally be run together (Console and WinForms).
  - This blog allows users to register and log in. The password is encrypted. It also allows visitors to see the posts, and authenticated users to create, edit, and delete their own posts.
- It uses Entity Framework as an ORM, and I am using SQLite so it can be tested more easily.
- SignalR is being used to handle WebSockets operations.

## Project Setup
- Clone the [repository](https://github.com/richard-alves/Blog-RWA.git).
- For it to work, it must be run with HTTP configuration because it is configured to run on port 5000, as the WebSockets connect to this port as well.

  ![image](https://github.com/user-attachments/assets/02beefd9-07eb-41bf-8266-66b99737eb92)

- The solution has three projects (API as server, and Console and WinForms as clients).
  - Only the Blog-RWA API is required to run, so you can execute it with just this project selected.
  - But you can choose to start all the projects together, just to see the client applications listening and receiving the notifications.

    ![image](https://github.com/user-attachments/assets/72c5312e-cc54-4d2b-8808-43ade809c1d1)

## Application Usage
- Run the project
- You can register, log in, see all users and all the posts without being authenticated. The other endpoints will require you to be authenticated.
- When you register, the application will return a token, so you can use this token to perform other operations.
- You can log in as well, and it will return the token again.
- **Use the token to authorize inside in the Swagger using 'bearer [token]'**

  ![image](https://github.com/user-attachments/assets/883c17b6-1e72-4adc-b92f-b91380fda228)

- Create a new post:

  ![image](https://github.com/user-attachments/assets/a8f08c3c-f5f8-4e72-946b-8eceaf284a1a)

- You can see the notification in the API build console:

  ![image](https://github.com/user-attachments/assets/c255514d-9d86-4a74-b354-87af1494b177)

- In the client Console Application:

  ![image](https://github.com/user-attachments/assets/49026f94-dcbd-474a-9821-e3cb2314e2df)

- And in the client WinForms Application:

  ![image](https://github.com/user-attachments/assets/66f74c1b-ce59-4a00-8967-a66fcda5a80a)

- Just run the other endpoints as necessary. The user will not be able to update or delete posts that were not created by them.
