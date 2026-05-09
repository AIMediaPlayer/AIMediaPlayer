using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPresenter
{
    public interface IPresenter
    {
        Media Next();

        Media Previous();

        Media Shuffle();
    }
}
