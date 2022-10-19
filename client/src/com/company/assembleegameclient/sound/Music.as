﻿package com.company.assembleegameclient.sound {
import com.company.assembleegameclient.parameters.Parameters;

public class Music {
    private static var musicName:String;
    private static var song:Song;

    public static function load(name:String):void {
        if (musicName == name) {
            return;
        }

        musicName = name;

        if (Parameters.data_.playMusic) {
            transitionNewMusic();
        }
    }

    private static function transitionNewMusic():void {
        if (song) {
            song.stop();
        }

        if (musicName == null || musicName == ""
                || Parameters.data_.musicVolume == 0 || !Parameters.data_.playMusic) {
            return;
        }
        song = new Song(musicName);
        if(musicName == "UTMenu") //hurt ears
        {
            song.play(Parameters.data_.musicVolume / 5);
        }
        else
        {
            song.play(Parameters.data_.musicVolume);
        }

    }

    public static function setPlayMusic(play:Boolean):void {
        Parameters.data_.playMusic = play;
        Parameters.save();

        if (play) {
            transitionNewMusic();
        }

        else if (song) {
            song.stop(true);
            song = null;
        }
    }

    public static function setMusicVolume(newVol:Number):void {
        Parameters.data_.musicVolume = newVol;
        Parameters.save();

        if (newVol == 0) {
            if (song) song.stop(true);
            return;
        }

        if (Parameters.data_.playMusic && song) {
            song.volume = newVol;
        }
    }
}
}
