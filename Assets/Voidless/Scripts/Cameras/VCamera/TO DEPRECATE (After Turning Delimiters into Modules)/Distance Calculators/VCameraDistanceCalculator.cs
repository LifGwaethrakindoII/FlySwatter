﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	[RequireComponent(typeof(VCameraDistanceAdjuster))]
	public abstract class VCameraDistanceCalculator : VCameraComponent
	{
		private VCameraDistanceAdjuster _distanceAdjuster; 	/// <summary>VCameraDistanceAdjuster's Component.</summary>

		/// <summary>Gets distanceAdjuster Component.</summary>
		public VCameraDistanceAdjuster distanceAdjuster
		{ 
			get
			{
				if(_distanceAdjuster == null) _distanceAdjuster = GetComponent<VCameraDistanceAdjuster>();
				return _distanceAdjuster;
			}
		}

		/// <summary>Gets Calculated distance towards given target.</summary>
		/// <param name="_target">Target.</param>
		/// <returns>Calculated distance towards given target.</returns>
		public abstract float GetCalculatedDistance(Vector3 _target);
	}
}