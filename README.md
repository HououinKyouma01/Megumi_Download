# Megumi_Download
Anime downloader &amp; automatic demux, subtitle replace and mux

This program performs 2 basic functions. More detailed description below. 

TL;DR
1. Downloading anime episodes from seedbox and moving them to folders with series name/season number

OR

2. Moving locally downloaded files to folders with the series name and cataloging by season number

3. Cataloging files and renaming them accordingly in compliance with kodi and TVDB naming format. 

4. **Automatic replacing words/names in subtitles** (using regex) based on custom list and automatic remux on download/move (useful to restore original Japanese name order, to correct name romanization, replace localized words etc).




**BACKGROUND**

I wrote this program with my setup in mind, as I could not find anything that suited me.

**I point out in advance that I am not a programmer, so probably many things could have been written more optimally, concisely and legibly, but what should work, does work.**

The first idea was to just download anime episodes from the seedbox and move them to the appropriate folders with the series name, season number and name them in such a way that they are recognizable by Kodi.

This I managed to realize years ago, but I didn't see the point of sharing it with the world. 
Lately, I've been getting more and more irritated seeing pointless changes in the official anime subs. This forced me to manually demux the mkv files, edit the files with the help of find and replace and re-mux. It was taking some time, so I thought to streamline my code to do it myself.

**How to use**

Now I will briefly explain how it works.

First - you need 2 external tools (if you want to use replace feature): 
- mkvextract.exe 
- mkvmerge.exe

Both included in [MKVToolNix](https://www.fosshub.com/MKVToolNix.html)

They should be placed in a directory together with the main executable file of the MegumiDownload.

Next we have the configuration files. Sample files are included in the "release"

 - config.megumi


Here we specify the parameters of the SSH/SFTP connection to our seedbox. **REMOTEPATH** - the path on the server where the files are located. **LOCALPATH** - where our local anime library is located. **KODISWITCH** - whether you want a message to appear when the download is finished in KODI (and then the data to connect to the local KODI server). **SAVEINFO** - the ability to save the original file names to a txt file in the folder with the season of the series. **MOVELOCAL** - puts the program into the mode of transferring locally downloaded files completely bypassing downloads from an external server. In this case, enter the path where the files are downloaded into the **LOCALTEMP** variable. **RENAME** - is a parameter that determines whether the program should rename downloaded files to those compatible with TVDB and Kodi or leave the original ones. 
> USER=user 
> 
> PASSWORD=pass 
> 
> HOST=hostname 
> 
> REMOTEPATCH=/home/user/data/
> 
> LOCALPATCH=D:\ 
> 
> KODISWITCH=OFF 
> 
> KODIADDRESS=192.168.1.1 
> 
> KODIPORT=80
> 
> KODIUSER=test 
> 
> KODIPASS=test 
> 
> SAVEINFO=ON
> 
> MOVELOCAL=OFF
> 
> LOCALTEMP=C:\Downloads\
> 
> RENAME=ON

 - groups.megumi


This is where fansub groups should be listed. One per line. For example:

> Doki
> 
> SNSbu
> 
> Nyanpasu
> 
> Chihiro
> 
> Nii-sama
> 
> MTBB

 - serieslist.megumi


Here the titles of the series we are watching - one per line. 3 parameters separated by "|" are required.
Name of File|Name of Folder With Series|Number of Season. For example:

> Yama no Susume - Next Summit|Yama no Susume|4


**If you want to make changes in the subtitles (e.g., changing the order of names), you need to create replace.txt files in the folder with the specific season of the series, for example:**

> D:\Anime\Bocchi the Rock\Season 1\replace.txt


In the content, enter what specifically you want to substitute in the format - the first parameter is the source data, the second is what you want to substitute with separated by "I" without spaces

First Name|First Name

Here an example from Bocchi the Rock in my case:

> Goto|Gotou
> 
> Ryo|Ryou
> 
> Honjo|Honjou
> 
> Fuko|Fuuko
> 
> Otsuki|Ootsuki
> 
> Hitori Gotou|Gotou Hitori
> 
> Nijika Ijichi|Ijichi Nijika
> 
> Ryou Yamada|Yamada Ryou
> 
> Ikuyo Kita|Kita Ikuyo
> 
> Yuyu Uchida|Uchida Yuyu
> 
> Seika Ijichi|Ijichi Seika
> 
> Michiyo Gotou|Gotou Michiyo
> 
> Fuuko Honjou|Honjou Fuuko
> 
> Akubi Hasegawa|Hasegawa Akubi
> 
> Eliza Shimizu|Shimizu Eliza
> 
> Kikuri Hiroi|Hiroi Kikuri
> 
> Shima Iwashita|Iwashita Shima
> 
> Yoyoko Ootsuki|Ootsuki Yoyoko
> 
> Akubi Hasegawa|Hasegawa Akubi


