# NetChat
This Program is an attempt at learning how to communicate over the Internet. The way I wish to accomplish this is to write a Messaging System with a Client-Server model.

Currently, the Server is semistable, while the Client is working solidly. This, however, is mainly because the program is very simple and debugging was easy.
The Files are those of the entire project, so feel free to edit it.

# The Client
The Client can send the Username, Messages and that's it(don't blame me, I didn't have any Idea what to add at the start of the project, where I missed a lot of knowledge!).
It can also start the server up, which it does by starting up a hardcoded Path. This will be changed in the future. Be aware that this means that your client will not start the server.
Currently, it also contains some smaller debug items. Please just ignore them, I was too lazy to remove them.

# The Server
The Server is the one that does some HEAVY lifting. It can handle all the users(sepperate threads are created for each one) and it keeps on scanning.
It will also keep track of the user being connected and will remove them from the list if they are disconnected.
It will, of course, reroute the message to all users(It's a server, duh!)
Basically, the server is, while seeming basic, the thing I am most proud of in this project.
