using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppComedor.Models
{
	public class GoogleUser
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public Uri Picture { get; set; }

	}
	public interface IGoogleManager
	{
		void Login(Action<GoogleUser, string> OnLoginComplete);
		void Logout();
	}
}