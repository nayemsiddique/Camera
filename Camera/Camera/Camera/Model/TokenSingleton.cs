using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Camera.Model
{
   public  sealed class TokenSingleton
    {
        private string ACCESS_TOKEN, REFRESH_TOKEN, TOKEN_TYPE, ACCOUNT_ID, IMGUR_USER_ACCOUNT, EXPIRES_IN;

        private static OAuth2Token token=null;
        private  static TokenSingleton instance= new TokenSingleton();
        public static  OAuth2Token GetToken()
        {
            //if (token == null)
            //    t=new TokenSingleton();
            //return token;

            return token;

            
        }


        private TokenSingleton()
        {
          

                 ACCESS_TOKEN = Application.Current.Properties["TOKEN_ACCESS"].ToString();
                 REFRESH_TOKEN = Application.Current.Properties["REFRESH_TOKEN"].ToString();
                 TOKEN_TYPE = Application.Current.Properties["TOKEN_TYPE"].ToString();

                ACCOUNT_ID = Application.Current.Properties["ACCOUNT_ID"].ToString();
                IMGUR_USER_ACCOUNT = Application.Current.Properties["IMGUR_USER_ACCOUNT"].ToString();

                EXPIRES_IN = Application.Current.Properties["EXPIRES_IN"].ToString();


                //#DisplayAlert("jj",EXPIRES_IN,"ok");


                token = new OAuth2Token(ACCESS_TOKEN, REFRESH_TOKEN, TOKEN_TYPE, ACCOUNT_ID, IMGUR_USER_ACCOUNT, int.Parse(EXPIRES_IN));

                if (token.ExpiresAt < DateTime.Now)
                {
                    RefreshToken();
                }
                Debug.WriteLine("Token="+token);


           
        }



        public void RefreshToken()
        {
            var client = new ImgurClient(MainPage.CLIENT_ID, MainPage.CLIENT_SECRET);
            var endpoint = new OAuth2Endpoint(client);
            var NEW_ACCESS_TOKEN = endpoint.GetTokenByRefreshTokenAsync(REFRESH_TOKEN);

            Application.Current.Properties["TOKEN_ACCESS"] = NEW_ACCESS_TOKEN;

            token = new OAuth2Token(NEW_ACCESS_TOKEN.ToString(), REFRESH_TOKEN, TOKEN_TYPE, ACCOUNT_ID, IMGUR_USER_ACCOUNT, int.Parse(EXPIRES_IN));
            
        }


        //private OAuth2Token CreateToken()
        //{
        //    var TOKEN_ACCESS = Application.Current.Properties["TOKEN_ACCESS"].ToString();
        //    var REFRESH_TOKEN = Application.Current.Properties["REFRESH_TOKEN"].ToString();
        //    var TOKEN_TYPE = Application.Current.Properties["TOKEN_TYPE"].ToString();

        //    var ACCOUNT_ID = Application.Current.Properties["ACCOUNT_ID"].ToString();
        //    var IMGUR_USER_ACCOUNT = Application.Current.Properties["IMGUR_USER_ACCOUNT"].ToString();

        //    var EXPIRES_IN = Application.Current.Properties["EXPIRES_IN"].ToString();


        //    //#DisplayAlert("jj",EXPIRES_IN,"ok");


        //    var token = new OAuth2Token(TOKEN_ACCESS, REFRESH_TOKEN, TOKEN_TYPE, ACCOUNT_ID, IMGUR_USER_ACCOUNT, int.Parse(EXPIRES_IN));
        //    //DisplayAlert("");
        //    return token;
        //}
    }
}
