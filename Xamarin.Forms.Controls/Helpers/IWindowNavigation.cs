using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Controls
{
    public interface IWindowNavigation
    {
		Task OpenNewWindowAsync();
		void NavegateToAnotherPage(Page page);	

	}
}
