﻿using System;

		/// <summary>

            // Check if data was written.
            // First check if emergency logger was started. We have to get a handle on the main
            // logger, which is available by now.
            var mainLogger = ((IPAFEmergencyServiceProvider<IPAFLoggingService>) logger).MainService;