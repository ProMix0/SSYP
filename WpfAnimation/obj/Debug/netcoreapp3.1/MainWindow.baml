   M S B A M L   `   `   `  ����G  CWpfAnimation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WpfAnimationWpfAnimation  5   	       WpfAnimation.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WpfAnimation     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��0.1* �6   �� 5      $��* �6   �� 5      $
��0.1* �6   .+�5   
   �� -   5         ��Name$ 	topCanvas��5      $��0��6$   $��0 0 50 0q�61   .+�5      �� -   5         ��Name$ topText��5      $��Top text��6)   $��60��69   $��Center=�6D    � Triggers 5      :� 5       :� RoutedEvent$ Loaded��6   .[�5      �� .��5      �� .��5      �� .��5   &   W�  W� From 166    g� Duration$ 00:00:5D�6v    �� 
TargetName$ topText��5   )   	 �� TargetProperty$	 (Canvas.Left)�5   )   
 g� AutoReverse$

 True��5   )    g� RepeatBehavior$ Forever��5   )   	 W� To 5   ?   ��  �� ElementName$ 	topCanvas�� �� Path$ ActualWidth� W� EasingFunction 5   &   0  *System.Windows.Media.Animation.QuinticEase  5   .   7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode$ 	EaseInOut=�6:   5   #   5      5       5!      5"      5$      5%      �� 5&   
   -   5        $ middleCanvas��5&      $��1��6'   $��0 0 0 30q�64   .+�5'      �� -   5         ��Name$ progressBorder��5'      
���  ��$	��112��6?   $��30��6K    �� CornerRadius$ 6��6W   $��3q�6h   ._�5(      �� -   5         ��Name$ progressBar��5(      
����  �$��1��6=   $
��Left=�6G   $��3q�6b    5)      :� 5*      $ Loaded��6'   .[�5+      �� .��5,   "   �� .��5-   &   �� .��5.   *   W�  16:   $ 00:00:20D�6}   $ progressBar��5/   )   $	 Width�50   )   $

 True��51   )   $ Forever��52   )    5.   C   �� $ middleCanvas��$ ActualWidth� 53   .   /  )System.Windows.Media.Animation.BounceEase  54   2      Bounces$ 6��6=   $ 	EaseInOut=�6I      
Bounciness$ 2Q�55   1   57   +   58   '   59   #   5:      5;      5=       5>      :� 5?      $ Loaded��6#   .[�5@      �� .��5A      �� .��5B   "   �� .��5C   &   W�  066   $ 00:00:17D�6z   $ progressBorder��5D   )   $	 (Canvas.Top)�5E   )   $

 True��5F   )   $ Forever��5G   )    5C   ?   �� $ middleCanvas��$ ActualHeight� 5H   *   4  .System.Windows.Media.Animation.ExponentialEase  5I   .      Exponent$ 6Q�6>   $ 	EaseInOut=�6K   5K   '   W� 5L   &    066    6716?   $ 00:00:11D�6H   $ progressBorder��5M   )   $	 (Canvas.Left)�5N   )   $

 True��5O   )   $ Forever��5P   )    5Q   *     5R   .   $ 3Q�6>   $ 	EaseInOut=�6K   5T   '   5U   #   5V      5W      5X      5Z      5[      �� 5\   
   -   5        $ buttomCanvas��5\      $��2��6'   $��0 0 52 0q�64   
����  �.+�5]      �� -   5        $ 
bottomText��5]      $��Bottom text��6,   $��Center=�6?    5^      :� 5_      $ Loaded��6#   .[�5`      �� .��5a      �� .��5b   "   �� .��5c   &   W�  166   $ 00:0:30D�6y   $ 
bottomText��5d   )   $	 (Canvas.Left)�5e   )   $

 True��5f   )   $ Forever��5g   )    5c   ?   �� $ buttomCanvas��$ ActualWidth� 5h   *   -  'System.Windows.Media.Animation.SineEase  5i   .   $ 	EaseInOut=�67   5k   '   5l   #   5m      5n      5o      5q      5r      5s      5t      