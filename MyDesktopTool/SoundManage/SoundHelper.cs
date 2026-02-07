using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

public class SoundHelper
{
    public void PlaySound(string OnePath)
    {
        MediaPlayer NewPlayer = new MediaPlayer();
        NewPlayer.Open(new Uri(OnePath));
        NewPlayer.Play();
        NewPlayer.Close();
    }
}
