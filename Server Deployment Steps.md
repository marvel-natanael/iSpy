# AWS Lightsail Desployment Steps #
[![image.png](https://i.postimg.cc/bJzCDYhw/image.png)](https://postimg.cc/sBN9brxk)
1. Make AWS account as root user and login
2. Choose Lightsail -> Create Instance
3. Configure your VM spesification
4. After done, choose your created VM -> Networking [![image.png](https://i.postimg.cc/sDkvVvY7/image.png)](https://postimg.cc/hf0Ss43t)
5. Add neceessary ports and protocols
6. Download the public key
7. Build your server
8. Upload using downloaded key, use [PuttyGen](https://www.puttygen.com/) or [WinSCP](https://winscp.net/eng/download.php)
9. Mark uploaded file as executable
10. Run the instance in the console

# Azure Deployment Steps #
1. Make Microsoft Account and login in Azure
2. In portal.azure.com choose virtual machine
[![d4dfd406-0869-45fa-b462-c6addd8e96a4.jpg](https://i.postimg.cc/13G7vvtr/d4dfd406-0869-45fa-b462-c6addd8e96a4.jpg)](https://postimg.cc/PCqMxz1C)
3. Fill in the spesifications according to your needs and then choose create
[![bcc45f01-0519-4fee-b399-0360a7235698.jpg](https://i.postimg.cc/T1jQtw2Y/bcc45f01-0519-4fee-b399-0360a7235698.jpg)](https://postimg.cc/hQj9jK7H)
4. After created, you should get a Public IP and Private IP

[![f0a24db9-a065-4c20-aaa2-3bab8c7ec265.jpg](https://i.postimg.cc/L5PG22Hr/f0a24db9-a065-4c20-aaa2-3bab8c7ec265.jpg)](https://postimg.cc/ZvTwLkvc)

5. Build your server & compress to zip

[![image.png](https://i.postimg.cc/vTvLWNX4/image.png)](https://postimg.cc/BPXK3Nr3)

6. For steps 7-9, please refer to https://mirror-networking.gitbook.io/docs/faq/server-hosting
7. Using command prompt, connect to Virtual Machine and upload the .zip file <br />
Command:
SCP filename.extension username@serveraddress:filepath <br />
8. Using Putty, unzip the files and mark server build as executable <br />
Command :<br />
unzip ./filename-demo.zip <br />
chmod +x ./filename-demo.x86_64 <br />
9. Run the server, now clients can connect to your game <br />
Command : <br />
./filename-demo.x86_64
