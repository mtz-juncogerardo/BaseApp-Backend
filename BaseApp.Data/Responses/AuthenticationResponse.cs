﻿namespace BaseApp.Data.Responses
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse(string token)
        {
            Token = token;
        }
        
        public string Token { get; }
    }
}