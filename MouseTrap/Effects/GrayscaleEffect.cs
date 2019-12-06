using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace MouseTrap.Effects
{
	public class GrayscaleEffect : ShaderEffect
	{
		private static readonly PixelShader _shader = new PixelShader()
		{
			// fxc /T ps_2_0 /E main /Fo"Grayscale.ps" Grayscale.fx
			UriSource = new Uri(@"pack://application:,,,/MouseTrap;component/Resources/Grayscale.ps")
		};

		public GrayscaleEffect()
		{
			PixelShader = _shader;
			UpdateShaderValue(InputProperty);
		}

		public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty("Input", typeof(GrayscaleEffect), 0);

		public Brush Input
		{
			get => (Brush)GetValue(InputProperty);
			set => SetValue(InputProperty, value);
		}
	}
}