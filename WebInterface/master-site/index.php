
<?php
include('dbconfig.php');

$connect = mysqli_connect($dbhost, $dbuser, $dbpass,$dbname) or die("Unable to connect to '$dbhost'");
$onlineUsers = mysqli_query($connect, "SELECT COUNT(1) FROM OnlineUsers");
$onlineSubscribers = mysqli_query($connect, "SELECT COUNT(1) FROM OnlineUsers WHERE Subscribed = 'YES'");
$onlineModerators = mysqli_query($connect, "SELECT COUNT(1) FROM OnlineUsers WHERE Moderator = 'YES'");
$activeAccounts = mysqli_query($connect, "SELECT COUNT(1) FROM Users");
$hasIntl = function_exists('numfmt_create');

if($hasIntl)
	$fmt = numfmt_create( 'en_US', NumberFormatter::DECIMAL );

?>

<HEAD>
  <TITLE>HORSE ISLE - Online Multiplayer Horse Game</TITLE>
  <META NAME="keywords"
    CONTENT="Horse Game Online MMORPG Multiplayer Horses RPG Girls Girly Isle World Island Virtual Horseisle Sim Virtual">
  <META NAME="description"
    CONTENT="A multiplayer online horse world where players can capture, train, care for and compete their horses against other players. A very unique virtual sim horse game.">
  <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon">
  <link rel="icon" href="/favicon.ico" type="image/x-icon">
  <link rel="meta" href="http://horseisle.com/labels.rdf" type="application/rdf+xml" title="ICRA labels" />
  <meta http-equiv="pics-Label"
    content='(pics-1.1 "http://www.icra.org/pics/vocabularyv03/" l gen true for "http://horseisle.com" r (n 0 s 0 v 0 l 0 oa 0 ob 0 oc 0 od 0 oe 0 of 0 og 0 oh 0 c 1)  gen true for "http://hi1.horseisle.com" r (n 0 s 0 v 0 l 0 oa 0 ob 0 oc 0 od 0 oe 0 of 0 og 0 oh 0 c 1))' />
  <style type="text/css">
    hr {
      height: 1;
      color: #000000;
      background-color: #000000;
      border: 0;
    }

    a {
      font: bold 14px arial;
      color: #6E3278;
    }

    TH {
      background-color: #EDE5B4;
      padding: 1px 6px;
      border: 2px dotted #6E3278;
      font: small-caps 900 14px arial;
      color: #000000;
    }

    TR.a0 {
      background-color: #EDE5B4;
    }

    TR.a1 {
      background-color: #D4CCA1;
    }

    TD {
      font: 14px arial;
      color: #000000;
    }

    TD.forum {
      font: 12px arial;
      color: #000000;
    }

    TD.forumlist {
      padding: 1px 6px;
      border: 2px dotted #6E3278;
      background-color: #EDE5B4;
      text-align: center;
      font: bold 14px arial;
      color: #000000;
    }

    TD.forumpost {
      padding: 5px 10px;
      border: 2px dotted #6E3278;
      background-color: #EDE5B4;
      text-align: left;
    }

    TD.adminforumpost {
      padding: 5px 20px;
      border: 2px dotted #6E3278;
      background-color: #BFE9C9;
      text-align: left;
    }

    TD.newslist {
      padding: 4px 4px;
      border: 2px dotted #6E3278;
      background-color: #FFDDEE;
      text-align: left;
      font: 14px arial;
      color: #000000;
    }

    FORUMSUBJECT {
      font: bold 14px arial;
      color: #004400;
    }

    FORUMUSER {
      font: 12px arial;
      color: #000044;
    }

    FORUMDATE {
      font: 12px arial;
      color: #444444;
    }

    FORUMTEXT {
      font: 14px arial;
      color: #440000;
    }
  </style>
</HEAD>

<BODY BGCOLOR=E0D8AA>
  <TABLE BORDER=0 CELLPADDING=0 CELLSPACING=0 WIDTH=100%>
    <TR WIDTH=100%>
      <TD WIDTH=512 ROWSPAN=3><A HREF= /><IMG SRC=/web/hoilgui1.gif ALT="Welcome to Horse Isle" BORDER=0></A></TD>
      <TD WIDTH=100% BACKGROUND=/web/hoilgui2.gif>&nbsp; </TD> <TD WIDTH=29><IMG SRC=/web/hoilgui3.gif> </TD> </TR> <TR>
      <TD WIDTH=100% BACKGROUND=/web/hoilgui4.gif align=right>
        <B>

          <TABLE CELLPADDING=0 CELLSPACING=2 BORDER=0>
            <FORM METHOD=POST ACTION=/account.php> <TR>
              <TD><B>USER:</B></TD>
              <TD><INPUT TYPE=TEXT SIZE=14 NAME=USER></TD>
    </TR>
    <TR>
      <TD><B>PASS:</B></TD>
      <TD><INPUT TYPE=PASSWORD SIZE=14 NAME=PASS></TD>
    </TR>
    <TR>
      <TD></TD>
      <TD><INPUT TYPE=SUBMIT VALUE=LOGIN> (<A HREF=/web/forgotpass.php>Forgot? </A>) </TD> </TR> </FORM> </TABLE> </TD>
          <TD WIDTH=29><IMG SRC=/web/hoilgui5.gif> </TD> </TR> <TR>
      <TD WIDTH=100% BACKGROUND=/web/hoilgui6.gif>&nbsp; </TD> <TD WIDTH=29><IMG SRC=/web/hoilgui7.gif> </TD> </TR>
          </TABLE> <CENTER>


        <CENTER>
          <TABLE WIDTH=90% BORDER=0>
            <TR>
              <TD>
                <FONT FACE=Verdana,arial SIZE=-1>
                  <TABLE>
                    <TR>
                      <TD WIDTH=100%>
                        <FONT FACE=Verdana,arial SIZE=-1>
                          <TABLE BORDER=0 CELLPADDING=3>
                            <TR>
                              <TD>
                                <CENTER><A HREF=/web/rules.php?ACCEPT=1> <IMG src=/web/screenshots/createaccount.png
                                    BORDER=0><BR>Create a FREE Account</A>
                              </TD>

                              <TD>
                                <CENTER><B>
                                    <FONT COLOR=880000>OR Log into your existing Horse Isle account at upper right
                                    </FONT>
                              </TD>

                              <TD>
                                <CENTER>OR Give a <A HREF=web/giftmembership.php>Gift Membership or Bonus</A> to an
                                    existing player</CENTER> </TD> </TR> </TABLE> <BR>
                                    <B>Fan Art Competition Winners: <A HREF="http://horsesareawesome.com/">View Fan
                                        Art</A></B> Contests from 2009 and 2011.<BR>
                                    <BR>
                                    <B>Parents!</B> Please click for some important information: <A
                                      HREF=/web/parents.php>Parent's Guide</A> <BR>
                                      <BR>

                                      <B>CURRENTLY:</B><BR>
                                      <FONT COLOR=550000><B>
					<?php 
						$onlineUsersCount = $onlineUsers->fetch_row()[0];
						if($hasIntl)
							echo numfmt_format($fmt, $onlineUsersCount);
						else
							echo $onlineUserCount;
					?></B></FONT> Players Online Now<BR>
                                      <FONT COLOR=550000><B><?php 
						$onlineSubscribersCount = $onlineSubscribers->fetch_row()[0];
						if($hasIntl)					
							echo numfmt_format($fmt, $onlineSubscribersCount);
						else
							echo $onlineSubscribersCount;
					?></B></FONT> Subscribers Online Now<BR>
                                      <FONT COLOR=550000><B>
					<?php 
						$onlineModeratorsCount = $onlineModerators->fetch_row()[0];
						if($hasIntl)					
							echo numfmt_format($fmt, $onlineModeratorsCount );
						else
							echo $onlineModeratorsCount;
					?></B></FONT> Moderators Online Now<BR>
                                      <FONT COLOR=550000><B>
					<?php 
						$activeUserCount = $activeAccounts->fetch_row()[0];
						if($hasIntl)						
							echo numfmt_format($fmt,  $activeUserCount);
						else
							echo $activeUserCount;
					?></B></FONT> Active Accounts<BR><BR>
                                      <B>ABOUT:</B><BR>
                                      Horse Isle is a vast multi-player horse based world. It allows for many players to
                                      interact while searching for wild
                                      horses roaming the lands. Once you have a horse, you can train it, take care of
                                      it, and compete with other players. Although the world graphics are simple 2D,
                                      they have been beautifully designed to create an interesting and vast world to
                                      explore. This land is completely non-violent. A great place for any aged player to
                                      have fun.
                                      <BR>

                              </TD>
                              <TD WIDTH=416 VALIGN=top>
                                <CENTER><embed src=web/frontpage.swf width=416 height=288><BR>
                                  <FONT FACE=Verdana,arial SIZE=-1 COLOR=880000>
                                    (If you can see the scene above, it should tell you if you have the necessary
                                    software required to play the game)<BR>
                                    (If not, you would need to download/upgrade for free <A
                                      HREF=http://www.adobe.com/products/flashplayer/>Flash Player</A>) </FONT> </TD>
                                      </TR> </TABLE> <BR><B>COSTS:</B><BR>
                                      You can play Horse Isle for FREE! or $5/mo USD game memberships provide many <A
                                        HREF="web/reasonstosubscribe.php">benefits</A>.<BR>

                                      <BR><B>FEATURES:</B><BR>
                                      Several different entertaining game activities:<BR>
                                      <UL>
                                        <LI>Capturing, training, and competing with your horses. These involve racing,
                                          jumping, dressage - all many-player games. Winning these events takes a
                                          combination of your horse's abilities, and your skill at the particular game.
                                        </LI>
                                        <LI>Completing mini-games throughout the world for fun and game money. Many are
                                          multiplayer also.</LI>
                                        <LI>Solving story-based quests and adventures by talking with characters in the
                                          game. There is a large variation of quests, from buried treasure, labyrinths,
                                          and painting, to simply returning someone's books!</LI>
                                        <LI>Buying and building up your very own ranch, making a piece of Horse Isle
                                          your very own!</LI>
                                        <LI>Naming and taking care of your horses. Finding them better tack, or even
                                          finding your horse a nice pet!</LI>
                                        <LI>Interacting with other players via chat, private chat, postal messages,
                                          actions, trading, competitive mini-games, and cooperative mini-games. Group
                                          activities include drawing rooms, music rooms, and poetry rooms!</LI>
                                        <LI>Searching the world for buried treasures, rare items, and hidden adventures.
                                        </LI>
                                        <LI>Trying to get the highest score or best times of many different tracked
                                          games.</LI>
                                      </UL>
                                      Ever-expanding content within the world includes:<BR>
                                      <UL>
                                        <LI>20+ unique communities located on different islands and climates. With
                                          unique weather systems.</LI>
                                        <LI>Over 100 unique horse breeds, very detailed with professional renderings of
                                          each breed in each color. More added regularly.</LI>
                                        <LI>500+ computer characters (residents) which you can interact with to complete
                                          adventures and learn things.</LI>
                                        <LI>500+ Adventure Quests. Completing these can earn you awards and bonuses.
                                        </LI>
                                        <LI>Hundreds of unique objects that can be found in the world or handled during
                                          quests.</LI>
                                        <LI>60+ unique minigames, many horse-based. </LI>
                                        <LI>Many completely original soundtracks and game musics, professionally
                                          produced.</LI>
                                      </UL>
                                      <BR>
                                      <B>REQUIRES:</B><BR>
                                      The game requires Flash 8. So any computer with Flash 8 should be able to run it
                                      (PC, MAC, and Linux (with Flash Player 9)).
                                      However it does use a lot of graphics, so a slow computer may have troubles. Any
                                      computer bought in the last 3-5 years should be fine. It also needs quick
                                      Internet, so dialup users may not enjoy the game fully.

                                      <BR><BR><B>SCREEN SHOTS:</B> (Pause your mouse over an image to popup details)<BR>
                                      <CENTER>
                                        <IMG SRC=web/screenshots/screen1.png
                                          ALT="The Group Drawing Room. Great place for pictionary, tictactoe or just drawing!"
                                          TITLE="The Group Drawing Room. Great place for pictionary, tictactoe or just drawing!">
                                        <IMG SRC=web/screenshots/screen2.png
                                          ALT="The Library. Viewing one of the breeds of Horse Isle."
                                          TITLE="The Library. Viewing one of the breeds of Horse Isle.">
                                        <IMG SRC=web/screenshots/screen3.png
                                          ALT="Treeton. One of the towns in Horse Isle."
                                          TITLE="Treeton. One of the towns in Horse Isle.">
                                        <IMG SRC=web/screenshots/screen7.png BORDER=1
                                          ALT="Quite the gathering of horse riders in the Desert!"
                                          TITLE="Quite the gathering of horse riders in the Desert!">
                                        <IMG SRC=web/screenshots/screen8.png BORDER=1
                                          ALT="A cold little community in Horse Isle."
                                          TITLE="A cold little community in Horse Isle.">
                                        <IMG SRC=web/screenshots/screen9.png BORDER=1
                                          ALT="A Drawing Room competition area on Art Isle."
                                          TITLE="A Drawing Room competition area on Art Isle.">
                                        <IMG SRC=web/screenshots/screen10.png BORDER=1
                                          ALT="Giant flowers and Rainbows make Flower Isle a special Island to visit."
                                          TITLE="Giant flowers and Rainbows make Flower Isle a special Island to visit.">
                                        <IMG SRC=web/screenshots/screen11.png BORDER=1
                                          ALT="One of the Arena MiniGames, This is Horse Racing."
                                          TITLE="One of the Arena MiniGames, This is Horse Racing.">


                                        <BR>
                              </TD>
                            </TR>
                          </TABLE>
                          <CENTER>
                            Horse Isle tested and developed using Firefox Browser<BR>

                            <TABLE BORDER=0 CELLPADDING=0 CELLSPACING=0 WIDTH=100%>
                              <TR>
                                <TD><IMG SRC=/web/hoilgui10.gif> </TD> <TD WIDTH=100% BACKGROUND=/web/hoilgui11.gif>
                                    </TD> <TD><IMG SRC=/web/hoilgui12.gif> </TD> </TR> </TABLE> <CENTER><B>
                                    [ <A HREF=//master.horseisle.com/beginnerguide/>New Player Guide</A> ]<BR>
                                      [ <A HREF=/web/rules.php>Rules </A> ] [ <A HREF=/web/termsandconditions.php>Terms
                                        and Conditions</A> ] [ <A HREF=/web/privacypolicy.php>Privacy Policy</A> ]</B>
                                        <BR>
                                        [ <A HREF=/web/expectedbehavior.php>Expected Behavior</A> ] [ <A
                                          HREF=/web/contactus.php>Contact Us</A> ] [ <A HREF=/web/credits.php>Credits
                                          </A> ]<BR>
                                          <FONT FACE=Verdana,Arial SIZE=-2>Copyright &copy; 2020 Horse Isle</FONT>

                                          <!-- Google Analytics -->
                                          <script src="http://www.google-analytics.com/urchin.js"
                                            type="text/javascript">
                                          </script>
                                          <script type="text/javascript">
                                            _uacct = "UA-1805076-1";
                                            urchinTracker();
                                          </script>
