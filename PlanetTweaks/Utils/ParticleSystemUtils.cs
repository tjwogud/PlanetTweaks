using UnityEngine;

namespace PlanetTweaks.Utils
{
	public static class ParticleSystemUtils
	{
		public static void ApplyColor(this ParticleSystem particleSystem, Color baseColor, Color startColor)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.startColor = new ParticleSystem.MinMaxGradient(startColor);
			Color color = Color.Lerp(baseColor, Color.black, 0.4f);
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
			new GradientColorKey(baseColor, 0f),
			new GradientColorKey(baseColor * new Color(0.6f, 0.6f, 0.6f), 1f)
			}, new GradientAlphaKey[]
			{
			new GradientAlphaKey(0.4f, 0f),
			new GradientAlphaKey(0f, 1f)
			});
			Gradient gradient2 = new Gradient();
			gradient2.SetKeys(new GradientColorKey[]
			{
			new GradientColorKey(color, 0f),
			new GradientColorKey(color * new Color(0.6f, 0.6f, 0.6f), 1f)
			}, new GradientAlphaKey[]
			{
			new GradientAlphaKey(0.6f, 0f),
			new GradientAlphaKey(0f, 1f)
			});
			colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient, gradient2)
			{
				mode = ParticleSystemGradientMode.TwoGradients
			};
		}

		public static void ApplySparkColor(this ParticleSystem particleSystem, Color minColor1, Color minColor2, Color maxColor1, Color maxColor2)
		{
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
			new GradientColorKey(minColor1, 0),
			new GradientColorKey(minColor2, 1)
			}, new GradientAlphaKey[]
			{
			new GradientAlphaKey(1, 0),
			new GradientAlphaKey(0, 1)
			});
			Gradient gradient2 = new Gradient();
			gradient2.SetKeys(new GradientColorKey[]
			{
			new GradientColorKey(maxColor1, 0),
			new GradientColorKey(maxColor2, 1)
			}, new GradientAlphaKey[]
			{
			new GradientAlphaKey(1, 0),
			new GradientAlphaKey(0, 1)
			});
			colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient, gradient2)
			{
				mode = ParticleSystemGradientMode.TwoGradients
			};
		}
	}
}
