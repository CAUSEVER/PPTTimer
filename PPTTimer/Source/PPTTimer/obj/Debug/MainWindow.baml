   M S B A M L   `   `   `  ����C  ?PPTTimer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PPTTimerPPTTimer  5   	   C&clr-namespace:PPTTimer.controls.StaticPPTTimer.controls.Static  6.   E'clr-namespace:PPTTimer.controls.DynamicPPTTimer.controls.Dynamic  6d       PPTTimer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:PPTTimer     5   	   9Static&clr-namespace:PPTTimer.controls.Static     6.   ;Dynamic'clr-namespace:PPTTimer.controls.Dynamic     6d     � Title$  	CountDown��5	   	   $	��100��6   $	��200��6(    J� Opacity$	 0.8Q�5
   	   ��5      �� 5   
    �� 
TargetType" �6   .�5      ��  �� Property �@�WindowStyle �� Value$
 None=�6,   �� 5       �@�AllowsTransparency . 63   �� 5       � ��
 ���� 6+   5      '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      ResizeBorderThickness$ 6q�6      CaptionHeight$ 0Q�61   .��5      � -   5         �Name$ MyGrid��5      $��0q�6   }�. .+�5   
   +   %PPTTimer.controls.Static.StaticButton  -   5        	  Name$	 close��5      +   %PPTTimer.controls.Static.StaticButton
   MouseLeftButtonDown$
 close_MouseLeftButtonDown��6w   $��50��6-   $��50��68    K� ZIndex$ 3��6D   $y�Hidden=�6U    � Cursor$
 Hand��6i      
FrontImage$ /pic/closeMouseover.png��5         BehindImage$ /pic/close.png��5      F� 5   
   -   5        $��ExitPath��5      
�������$ 1Q�6,   $ 2��68   ��5      @� 5      -   5        ExitEllipse�� @� RadiusX$ 0Q�5   7    @� RadiusY 6C   ��  �� RelativeSource ��  �� Mode$
 Self=� �� Path$ RadiusX�5      5      �� 5!   
   -   5         ��Name$ mintime��5!      $��35��6#   $��21��6.   $	��123��6:   $ 2��6E   $��16(�6V   $��Center=�6d   $��	-50,0,0,0q�6�   $��Center=�6�   $��Center=�6�   $y�Hidden=�6�   �� 5"   
   -   5        $ sectime��5"      $��35��6#   $��21��6.   $	��456��6:   $ 2��6E   $��16(�6V   $��Center=�6d   $��50,0,0,0q�6�   $��Center=�6�   $��Center=�6�   $y�Hidden=�6�   �� 5$   
   -   5        $ tipText��5$      $ 1Q�5%      $��Center=�5&   	   $��Center=�6&   $��20(�5'      $	��100��5(      $
��auto��6   $ 1��6*   $y�Hidden=�6;   .��6O   5)      �� 5+   
   -   5         ��Name$ 	baseplate��5+       �� CornerRadius$ 10��6$   $��6q�66   ��5,      �� 5-       �� 
StartPoint$ 0.5,0&�6&    �� EndPoint$ 0.5,0.1&�69   .��5.      �  � Color$ 	#FFEAF4FF��6#    � Offset$ 0Q�65   � 5/      $ 	#FFF4F8F9��6#   $ 1Q�65   50       J� Effect 53      3  -System.Windows.Media.Effects.DropShadowEffect  54         ShadowDepth$ 0Q�6#      
BlurRadius$ 10Q�63      Color$ #AAAAAA��6C   ._�57      �  � ShowGridLines .  .+�5;      �� -	   5          ��Name$
  time��5;      $��-20:00��6*   $��Center=�68   $��Center=�6S   $��62(�6p   $��0,0,0,0q�6~   $ 0��5<      �� 5>      ! �� Orientation$! 
Horizontal=�6   $	��Top=�66   .+�5?      �� -
   5        " ��Name$
" icon��5?      $��/pic/PPTTimer.png��6*   $��15��6E   $��15��5@      $
��Left=�6(   $��	4,6.5,0,0q�5A      5B      �� 5D      $! 
Horizontal=�6   $��RightToLeft=�66   $	��Top=�6R   .+�5E        -   5        $
	 exit��5E   *   $
 exit_MouseLeftButtonDown��5J   (   $��28��5F   "   $��28��6.   $
 Hand��5G   "   $ /pic/exit.png��5H   (   $ /pic/exitMouseover.png��5I   (   $ 1��5J   W   ��5G   0   Z� 5K        5M      -   5        $	 minimize��5M   *   $"
 minimize_MouseLeftButtonDown��5R   H   $
��Left=�5M   <   $��28��5N   "   $��28��6.   $
 Hand��69   # � ToolTip$?# 9隐藏窗口，隐藏后可以在任务栏处双击显示��5O   (   
���   �$  /pic/minimizeMouseover.png��5Q   (   $ /pic/minimize.png��5R   (   $ 1��5S   *   ��5O   F   Z� 5T      5U      � 5Y      .+�5Z      �� $! 
Horizontal=�6!   $��LeftToRight=�6:   .+�5[        -   5        $	 AddThing��5[   .   $"
 AddThing_MouseLeftButtonDown��5`   Q   $��20��5[   @   $��20��6L   
������ $
 Hand��6q   $'# !添加一个新的倒计时时间��5\   .   $��2,0,5,2q�5]   .   $��Bottom=�5^   .   $ /pic/add.png��5_   .   $ /pic/addMouseover.png��5`   .   $ 1��5a   .   -   'PPTTimer.controls.Dynamic.DynamicButton  5b      -   5        $  Name$$ StartAndStop��5b   0   -   'PPTTimer.controls.Dynamic.DynamicButton%   MouseLeftButtonDown$&%  StartAndStop_MouseLeftButtonDown��5c   &   $��20��5b   F   $��20��6R   
������ $
 Hand��5c   ]   $# 开始计时��5d   &   $��5,0,5,2q�65   $��Bottom=�5e   &   &   BehindStopImage$& /pic/stop.png��5f   &   '   BehindStartImage$' /pic/start.png��5g   &   (   FrontStopImage$( /pic/stopMouseover.png��5h   &   )   FrontStartImage$) /pic/startMouseover.png��5i   &   $ 1��5j   0     5k      -   5        $	 restart��5k   .   $!
 restart_MouseLeftButtonDown��5n   =   $��20��5k   ?   $��20��6K   
������ $
 Hand��6p   $# 双击重新计时��5l   ,   $��Bottom=�5m   ,   $��5,0,5,2q�5n   ,   $ /pic/restart.png��5o   ,   $ /pic/restartMouseover.png��5p   ,   $ 1��5q   .   5r        5t      -   5        $	 Setting��5t   *   $!
 Setting_MouseLeftButtonDown��5w   O   $��Right=�5t   ;   $��20��6W   $��20��6c   $��2q�6n   $
 Hand��6�   $ /pic/setting.png��5u   (   $��Bottom=�5v   (   $ /pic/settingMouseover.png��5w   (   $ 1��5x   *   ��5t   y   Z� 5y      5z      5|      5}      5~      5�      