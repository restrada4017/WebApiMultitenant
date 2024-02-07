using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPT.Application.Contracts.Infrastructure.Utilities;
using GPT.Application.CustomEntities;
using Library.KeyVault.Ideact;
using Microsoft.Extensions.Options;

namespace GPT.Utilities.Keys
{
    public class AzureKeyvault : IKeys
    {
        readonly bool _isUseKey = false;

        public AzureKeyvault(IOptions<KeyVaultEntity> options)
        {
            KeyVaultEntity _KeyVault = options.Value;
            if(!string.IsNullOrEmpty(_KeyVault.KvUri))
            {
                SecretKeyVaultFacade.SetConfiguration(_KeyVault.KvUri, _KeyVault.TenantId, _KeyVault.ClientId, _KeyVault.ClientSecret);
            }
            
            _isUseKey = !string.IsNullOrEmpty(_KeyVault.KvUri);
        }
        public Task<string> GetValueByKey(string key)
        {
            string valuestr = SecretKeyVaultFacade.GetSecret(key).Value;
            return Task.FromResult(valuestr);
        }
        public bool IsUseKey()
        {
            return _isUseKey;
        }
    }
}
