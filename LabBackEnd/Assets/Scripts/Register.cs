using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class Register : MonoBehaviour
{
    [SerializeField]
    public class RegisterRequestData
    {
        public string email;
        public string password;
        public string name;
        public string linkAvatar;
        public int regionId;

        public RegisterRequestData(string email, string password, string name, string linkAvatar, int regionId)
        {
            this.email = email;
            this.password = password;
            this.name = name;
            this.linkAvatar = linkAvatar;
            this.regionId = regionId;

        }
    }

    [SerializeField]
    public class ResponseUserError
    {
        public bool isSuccess;
        public string notification;
        public List<Error> data;
    }

    [SerializeField]
    public class Error
    {
        public string code;
        public string description;
    }

    [SerializeField]
    public class ResponseUserSuccess
    {
        public bool isSuccess;
        public string notification;
        public RegisterUserData data;
    }

    [SerializeField]
    public class RegisterUserData
    {
        public string name;
    }
}
