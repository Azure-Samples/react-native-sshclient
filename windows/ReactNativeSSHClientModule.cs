using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactNativeSSHClient
{
    public class ReactNativeSSHClientModule : ReactContextNativeModuleBase, ILifecycleEventListener
    {
        public ReactNativeSSHClientModule(ReactContext reactContext) : base(reactContext)
        {
        }

        public override string Name
        {
            get
            {
                return "SSH";
            }
        }

        [ReactMethod]
        public void Execute(JObject config, string command, IPromise promise)
        {
            try
            {
                JToken userValue = null;
                if (!config.TryGetValue("user", out userValue))
                {
                    throw new Exception("user property retrieval error");
                }

                string user = (string)userValue;

                JToken hostValue = null;
                if (!config.TryGetValue("host", out hostValue))
                {
                    throw new Exception("host property retrieval error");
                }

                string host = (string)hostValue;

                JToken passwordValue = null;
                if (!config.TryGetValue("password", out passwordValue))
                {
                    throw new Exception("password property retrieval error");
                }

                string password = (string)passwordValue;

                int timeoutInSeconds = 60;
                JToken timeoutValue = null;
                if (config.TryGetValue("timeout", out timeoutValue))
                {
                   timeoutInSeconds = (int)timeoutValue;
                }

                var connectionInfo = new PasswordConnectionInfo(host, user, password);
                connectionInfo.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);

                SshClient cSSH = new SshClient(connectionInfo);
                cSSH.Connect();
                var commandResult = cSSH.RunCommand(command);
                cSSH.Disconnect();
                promise.Resolve(commandResult.Result);
                return;
            }
            catch (Exception ex)
            {
                promise.Reject(ex);
                return;
            }
        }

        public void OnDestroy()
        {
            throw new NotImplementedException();
        }

        public void OnResume()
        {
            throw new NotImplementedException();
        }

        public void OnSuspend()
        {
            throw new NotImplementedException();
        }
    }
}
