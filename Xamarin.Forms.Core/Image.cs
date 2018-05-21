using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_ImageRenderer))]
	public class Image : View, IImageController, IElementConfiguration<Image>, IViewController, IImageElement
	{
		public static readonly BindableProperty SourceProperty = ImageElement.SourceProperty;

		public static readonly BindableProperty AspectProperty = ImageElement.AspectProperty;

		public static readonly BindableProperty IsOpaqueProperty = ImageElement.IsOpaqueProperty;

		internal static readonly BindablePropertyKey IsLoadingPropertyKey = BindableProperty.CreateReadOnly(nameof(IsLoading), typeof(bool), typeof(Image), default(bool));

		public static readonly BindableProperty IsLoadingProperty = IsLoadingPropertyKey.BindableProperty;

		public static readonly BindableProperty IsAnimationAutoPlayProperty = BindableProperty.Create(nameof(IsAnimationAutoPlay), typeof(bool), typeof(Image), false);

		internal static readonly BindablePropertyKey IsAnimationPlayingPropertyKey = BindableProperty.CreateReadOnly(nameof(IsAnimationPlaying), typeof(bool), typeof(Image), false);

		public static readonly BindableProperty IsAnimationPlayingProperty = IsAnimationPlayingPropertyKey.BindableProperty;

		readonly Lazy<PlatformConfigurationRegistry<Image>> _platformConfigurationRegistry;

		public Image()
		{
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<Image>>(() => new PlatformConfigurationRegistry<Image>(this));
		}

		public Aspect Aspect
		{
			get { return (Aspect)GetValue(AspectProperty); }
			set { SetValue(AspectProperty, value); }
		}

		public bool IsLoading
		{
			get { return (bool)GetValue(IsLoadingProperty); }
		}

		public bool IsOpaque
		{
			get { return (bool)GetValue(IsOpaqueProperty); }
			set { SetValue(IsOpaqueProperty, value); }
		}

		public bool IsAnimationPlaying
		{
			get { return (bool)GetValue(IsAnimationPlayingProperty); }
			private set { SetValue(IsAnimationPlayingPropertyKey, value); }
		}

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource Source
		{
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public bool IsAnimationAutoPlay
		{
			get { return (bool)GetValue(IsAnimationAutoPlayProperty); }
			set { SetValue(IsAnimationAutoPlayProperty, value); }
		}

		public void StartAnimation()
		{
			if (!IsSet(IsAnimationAutoPlayProperty))
				IsAnimationAutoPlay = false;

			IsAnimationPlaying = true;
		}

		public void StopAnimation()
		{
			if (!IsSet(IsAnimationAutoPlayProperty))
				IsAnimationAutoPlay = false;

			IsAnimationPlaying = false;
		}

		public event EventHandler AnimationFinishedPlaying;

		public void OnAnimationFinishedPlaying()
		{
			SetValue(IsAnimationPlayingProperty, false);
			AnimationFinishedPlaying?.Invoke(this, null);
		}

		protected override void OnBindingContextChanged()
		{
			ImageElement.OnBindingContextChanged(this, this);
			base.OnBindingContextChanged();
		}

		[Obsolete("OnSizeRequest is obsolete as of version 2.2.0. Please use OnMeasure instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
		{
			SizeRequest desiredSize = base.OnSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
			return ImageElement.Measure(this, desiredSize, widthConstraint, heightConstraint);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void SetIsLoading(bool isLoading)
		{
			SetValue(IsLoadingPropertyKey, isLoading);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool GetLoadAsAnimation()
		{
			return IsSet(Image.IsAnimationAutoPlayProperty) || IsSet(Image.IsAnimationPlayingProperty);
		}

		public IPlatformElementConfiguration<T, Image> On<T>() where T : IConfigPlatform
		{
			return _platformConfigurationRegistry.Value.On<T>();
		}

		void IImageElement.OnImageSourcesSourceChanged(object sender, EventArgs e) =>
			ImageElement.ImageSourcesSourceChanged(this, e);

		void IImageElement.RaiseImageSourcePropertyChanged() => OnPropertyChanged(nameof(Source));
	}
}