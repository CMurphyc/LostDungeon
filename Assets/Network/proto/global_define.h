#ifndef CONFIG_GLOBAL_DEFINE_H_
#define CONFIG_GLOBAL_DEFINE_H_

/*
    S2CType = C2SType + 100
    messageType: 
    1表示客户端的登录请求
    2表示客户端的注册请求
    ...
    101表示服务器处理客户端登录请求的返回消息
    102表示服务器处理客户端注册请求的返回消息
*/

#define BUFF_SIZE 2048
#define MAXEVENTS 64

#define HEAD_SIZE 8
#define LEN_SIZE 4
#define TYPE_SIZE 4

#define LOGIN_REQ 1
#define REGISTER_REQ 2
#define LOGIN_RET 101
#define REGISTER_RET 102
#define PER_FRAME_TIME 50000
#define PORT 10000

#endif
