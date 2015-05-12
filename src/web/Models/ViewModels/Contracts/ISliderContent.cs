using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxCommerceStarterKit.Web.Models.ViewModels.Contracts
{
    public interface ISliderContent
    {
        string Name { get; set; }
        string Description { get; set; }
        string ActionText { get; set; }
        string ActionLink { get; set; }

    }
}
