# C# - Blog API
## API Information
- A simple Blog API in C# / ASP.NET Core, with 2 client applications that can optionally be run together (Console and WinForms).
  - This blog allows user to registry and login, the password is cryptographed. It also allows visitor to see the posts, and authenticated users to create post, as well edit and delete their own posts.
- It is using Entity Framework as an ORM, and I am using SQLite, so it can be tested more easier.
- SignalR is being used to perform the WebSockets operations.

## Project Setup
- Clone the [repository](https://github.com/richard-alves/Blog-RWA.git).
- For it to work, it must be run with http configuration, because it is configured to run under port 5000, as the websockets are connecting to this port as well.

  ![image](https://github.com/user-attachments/assets/02beefd9-07eb-41bf-8266-66b99737eb92)

- The solution has 3 projects (API as server, and Console and WinForms as clients).
  - Only Blog-RWA API is required to run, so you can execute only with this project selected.
  - But you can choose to start all of the projects together, just to see the client applications listening and recieving the notifications

    ![image](https://github.com/user-attachments/assets/72c5312e-cc54-4d2b-8808-43ade809c1d1)

## Application Usage
- Run the project
- You can register, login, see all users and all the posts without being authenticated. The other endpoints will require you to be authenticated.
- When you register the application, it will return the token, so you can already use this token to perform the other operations.
- You can login as well it will return the token again.
- **Use the token to authorize inside in the Swagger using 'bearer [token]'**

  ![image](https://github.com/user-attachments/assets/883c17b6-1e72-4adc-b92f-b91380fda228)

- Create a new post:

  ![image](https://github.com/user-attachments/assets/a8f08c3c-f5f8-4e72-946b-8eceaf284a1a)

- You can see the notification in the API Build console

  ![image](https://github.com/user-attachments/assets/c255514d-9d86-4a74-b354-87af1494b177)

- In the client Console Application

  ![image](https://github.com/user-attachments/assets/49026f94-dcbd-474a-9821-e3cb2314e2df)

- And in the client WinForms Application

  ![image](https://github.com/user-attachments/assets/66f74c1b-ce59-4a00-8967-a66fcda5a80a)

- Just run the other endpoints as necessary, the user will not be able to update or delete posts the was not created by him



