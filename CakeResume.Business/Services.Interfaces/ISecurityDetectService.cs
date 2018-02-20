using System;
using System.Collections.Generic;
using System.Text;

namespace CakeResume.Business.Services.Interfaces
{
    public interface ISecurityDetectService
    {
		void Run();

		IList<string> GetSecurityKeywords();
    }
}
