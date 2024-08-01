using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIOClient;
using UnitySocketManager;
using UnityEngine.UI;
using UnityAuthStructure;
using MahJongController;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System.Linq;
using System.Data;

namespace MahjongAuthController
{
    public class AuthController : MonoBehaviour {
    
        public RectTransform SigninPanel;
        public RectTransform SigninUp;
        
        private RectTransform currentPanel;
        [SerializeField] private GameObject OpenModal;
        
        [SerializeField] private InputField signUpEmail;
        [SerializeField] private InputField signUpPass;
        [SerializeField] private InputField signUpPassConfirm;
        [SerializeField] private InputField signUpUserName;
        [SerializeField] private InputField email;
        [SerializeField] private InputField password;
        public WarningPanel warningPanel;
        public GameObject obj;


        private SocketIOUnity socket;
        private bool objFlag = false;
        const string ServerURL = "SignIn";

        public void SignIn()
        {
            if (string.IsNullOrEmpty(email.text))
            {
                warningPanel.Show(400, 200, "プレイヤーのメールアドレスを入力してください。");
                return;
            }
             if(IsValidEmail(email.text) == false){
                warningPanel.Show(400, 200, "メールの形式が正しくありません。");
                return;
            }
            if (string.IsNullOrEmpty(password.text) || password.text.Length< 6)
            {
                warningPanel.Show(400, 200, "正しいパスワードを入力してください");
                return;
            }

            StartCoroutine(SignInRequest(email.text, password.text));
        }

        public void SignUp()
        {
            if (string.IsNullOrEmpty(signUpEmail.text))
            {
                warningPanel.Show(400, 200, "プレイヤーのメールアドレスを入力してください。");
                return;
            }
            if (IsValidEmail(signUpEmail.text) == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(signUpPass.text) || signUpPass.text.Length < 6)
            {
                warningPanel.Show(400, 200, "正しいパスワードを入力してください");
                return;
            }

            if (string.IsNullOrEmpty(signUpUserName.text) || signUpUserName.text.Length < 6)
            {
                warningPanel.Show(400, 200, "正しいユーザー名を入力してください");
                return;
            }

            if (signUpPass.text != signUpPassConfirm.text)
            {
                warningPanel.Show(400, 200, "正しいパスワードを入力してください");
                return;
            }
            StartCoroutine(SignUpRequest(signUpUserName.text, signUpEmail.text, signUpPass.text));
        }   

        public static bool IsValidEmail(string email)
        {
            string pattern = Constant.ValPattern;
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public IEnumerator SignInRequest (string playerEmail, string playerPass){
            InitializeSocket(Constant.ServerURL);
            yield return new WaitForSeconds(3f);
            if (objFlag == false) {
                obj.SetActive(true);
                objFlag = true;
            }
            Communication communication = obj.GetComponent<Communication>();
            communication.LoginPlayer(playerPass, playerEmail, SocketManager.GetSocket());
        }

        public IEnumerator SignUpRequest (string playerUserName, string playerEmail, string playerPass){
            InitializeSocket(Constant.ServerURL);
            yield return new WaitForSeconds(2f);
            int randomAvatarNumber = UnityEngine.Random.Range(101, 105); // 81 because upper limit is exclusive
            string avatar = randomAvatarNumber.ToString();
            if (objFlag == false)
            {
                obj.SetActive(true);
                objFlag = true;
            }
            Communication communication = obj.GetComponent<Communication>();
            communication.CreateAccounts(playerUserName, playerPass, playerEmail, avatar, SocketManager.GetSocket());
        }

       private void InitializeSocket(string Url)
       {

            Debug.Log(Url);
            SocketIOUnity stateSocket = SocketManager.GetSocket();
            Debug.Log(stateSocket);
            if (stateSocket == null) {
            Debug.Log(Url);
                var uri = new Uri(Url);
                socket = new SocketIOUnity(uri);
                socket.OnConnected += SocketConnected;
                socket.Connect();
                SocketManager.SetSocket(socket);
            }
     }

        private void SocketConnected(object sender, EventArgs e)
        {
            Debug.Log("Connected Server");
        }

    }

}