﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HandBrake.ApplicationServices.Interop.Json.Scan;
using VidCoder.Extensions;
using VidCoder.Services;
using VidCoder.ViewModel;

namespace VidCoder.View
{
	/// <summary>
	/// Interaction logic for MultipleTitlesDialog.xaml
	/// </summary>
	public partial class QueueTitlesWindow : Window
	{
		private QueueTitlesWindowViewModel viewModel;

		public QueueTitlesWindow()
		{
			InitializeComponent();

			this.DataContextChanged += this.OnDataContextChanged;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			this.PlaceDynamic(Config.QueueTitlesDialogPlacement2);
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.viewModel = this.DataContext as QueueTitlesWindowViewModel;
			this.viewModel.PropertyChanged += this.viewModel_PropertyChanged;
		}

		private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "PreviewImage")
			{
				this.RefreshImageSize();
			}
		}

		private void RefreshImageSize()
		{
			var previewVM = this.DataContext as QueueTitlesWindowViewModel;
			if (previewVM.SelectedTitles.Count != 1)
			{
				return;
			}

			double widthPixels, heightPixels;

			SourceTitle title = previewVM.SelectedTitles[0].Title;
			if (title.Geometry.PAR.Num > title.Geometry.PAR.Den)
			{
				widthPixels = title.Geometry.Width * ((double)title.Geometry.PAR.Num / title.Geometry.PAR.Den);
				heightPixels = title.Geometry.Height;
			}
			else
			{
				widthPixels = title.Geometry.Width;
				heightPixels = title.Geometry.Height * ((double)title.Geometry.PAR.Den / title.Geometry.PAR.Num);
			}

			ImageUtilities.UpdatePreviewImageSize(this.previewImage, this.previewImageHolder, widthPixels, heightPixels);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Config.QueueTitlesDialogPlacement2 = this.GetPlacementXml();
		}

		private void previewImageHolder_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.RefreshImageSize();
		}
	}
}