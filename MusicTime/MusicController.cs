﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicTime
{
    class MusicController
    {
       private static MusicManager musicManager = MusicManager.getInstance;
       private static Device device             = Device.getInstance;

        public static async void PlayPauseTrackAsync()
        {
            try
            {
                if (SoftwareUserSession.GetSpotifyUserStatus())
                {
                    if(await MusicManager.isTrackPlayingAsync())
                    {
                        await MusicManager.SpotifyWebPauseAsync();
                        MusicTimeCoPackage.UpdateCurrentTrackOnStatusAsync(null);
                    }
                    else
                    {
                      
                        await MusicManager.SpotifyWebPlayAsync();
                        MusicTimeCoPackage.UpdateCurrentTrackOnStatusAsync(null);
                    }

                    if (!MusicManager.isDeviceOpened())
                    {
                        await LaunchPlayerAsync(new options());

                        await MusicManager.getDevicesAsync();


                    }


                }
            }
            catch (Exception ex)
            {

                
            }
            
           
          
        }


        public static async void PreviousTrackAsync()
        {
            try
            {
                if (SoftwareUserSession.GetSpotifyUserStatus())
                {
                    if (!MusicManager.isDeviceOpened())
                    {
                        await LaunchPlayerAsync(new options());

                        await MusicManager.getDevicesAsync();
                        
                    }


                    await MusicManager.SpotifyWebPlayPreviousAsync();
                   

                }

            }
            catch (Exception ex)
            {

                
            }
            
        }

        public static async void NextTrackAsync()
        {
            try
            {
                if (SoftwareUserSession.GetSpotifyUserStatus())
                {
                    if (!MusicManager.isDeviceOpened())
                    {
                        await LaunchPlayerAsync(new options());

                        await MusicManager.getDevicesAsync();
                        
                    }

                     await MusicManager.SpotifyWebPlayNextAsync();
                   
                }
            }
            catch (Exception ex)
            {

                
            }
           
        }

        public static async Task LaunchPlayerAsync(options options)
        {

            LaunchWebPlayerAsync(new options());

            Thread.Sleep(5000);
         
            if (!string.IsNullOrEmpty(options.playlist_id))
            {
               await MusicManager.SpotifyPlayPlaylistAsync(options.playlist_id, options.track_id);
            }
            else
            {
                await MusicManager.SpotifyPlayPlaylistAsync(null, options.track_id);
            }

        }

        private static void LaunchWebPlayerAsync(options options )
        {
            CodyConfig codyConfig   = CodyConfig.getInstance;
            string userID           = codyConfig.spoftifyUserId;

            if (SoftwareUserSession.GetSpotifyUserStatus())
            {
                if (!string.IsNullOrEmpty(options.album_id))
                {
                    string albumId = MusicUtil.CreateSpotifyIdFromUri(options.album_id);
                    SoftwareSpotifyManager.launchWebUrl("https://open.spotify.com/album/" + albumId);
                }
                else if (!string.IsNullOrEmpty(options.track_id))
                {
                    string trackId = MusicUtil.CreateSpotifyIdFromUri(options.track_id);
                    SoftwareSpotifyManager.launchWebUrl("https://open.spotify.com/track/" + trackId);

                }
                else if (!string.IsNullOrEmpty(options.playlist_id))
                {
                    string playlistId = MusicUtil.CreateSpotifyIdFromUri(options.playlist_id);

                    SoftwareSpotifyManager.launchWebUrl("https://open.spotify.com/playlist/" + playlistId);
                }
                else
                    SoftwareSpotifyManager.launchWebUrl("https://open.spotify.com/browse");


                MusicTimeCoPackage.UpdateEnablePlayercontrol(true);
            }
        }

       
      

     
    }
   


}
