   M S B A M L   `   `   `  ����T  PGraphAlgorithmsVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  5      K*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  5      *    $GraphAlgorithmsVisualizer.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      <local*clr-namespace:GraphAlgorithmsVisualizer.UI     5      ?controls*clr-namespace:GraphAlgorithmsVisualizer.UI     5      V RMaterialDesignThemes.Wpf, Version=2.5.1.1297, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        � Title$!  Graph Algorithms Visualizer��5	       � WindowStartupLocation$ CenterScreen=�5
       � WindowState$ 	Maximized=�5         MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#���  5       � 	Resources 5        Stop  
LeftButton �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property	 �� Value
 �� RelativeSource  
PlayButton �� 
TargetType  
StopButton  
FastButton 
	 Vertex *
 &MaterialDesignFloatingActionMiniButton�� 5   
   %�  &	       &	 *     7  &	 U    &	 	    &	 �    &		 v    7 
 ��5      Resources/stop.png��6;   �� 5      " d�6'   8   .�5      ��  � ��
	 �����6/   �� 5       � ��$	 5 5 5 5q�6,   �� 5       � ��
	 �����60   �� 5       � ��	 6-   �� *ActualHeight+
 �� *Self+�� 5       � ��	 6.   �� *ActualWidth+
 �� *Self+5      �� 5      " ��6'   
PlayButton��.�5      ��  � ��	 5       �� 5!      " ��6*   .R�5"      �� $��Resources/play.png��6$   6B   5#      5%      5&      �� 5'      " ��6'   
StopButton��.�5(      ��  � ��	 5)      �� 5*      " ��6*   .R�5+      �� $��Resources/stop.png��6$   6B   5,      5.      5/      �� 50      " ��6'   
FastButton��.�51      ��  � ��	 52      �� 53      " ��6*   .R�54      �� $��Resources/fast_play.png��6$   6G   55      57      58      �� 59      " ��6#   8   .�5:      ��  � ��
	 � �|�6/   �� 5;       � ��
	 �����60   �� 5<       � ��	 6-   �� *ActualHeight+
 �� *Self+�� 5=       � ��	 6.   �� *ActualWidth+
 �� *Self+5>      5?      .��5A      �  � RowDefinitions 5B   
   �� 5C      $
��30px �6   6,   �� 5D      6   �� 5E      $
��50px �6   6,    � ColumnDefinitions 5G   
   �� 5H      $
��50px �6   6.   �� 5I      6!   �� 5J      $��500px �6   6/   .+�5M   
   �� $��0��6   $��1��6%   $��2��62   .+�5N      d� -   5         d�Name$ CursorSwitch��5O      CursorSwitch��#��[ 5Q      $��5 5 5 5q�5R      .��5S      �� $��Resources/cursor.png��6    �� BitmapScalingMode$ HighQuality=�66   5T      d� 5U      -   5        $ VertexSwitch��5V      VertexSwitch��#��[ 5X      $��5 5 5 5q�5Y      .��5Z      �� $��Resources/vertex.png��6   $ HighQuality=�66   5[      d� 5\      -   5        $ 
EdgeSwitch��5]      
EdgeSwitch��#��[ 5_      $��5 5 5 5q�5`      .��5a      �� $��Resources/edge.png��6   $ HighQuality=�64   5b      d� 5c      -   5        $ OrientedEdgeSwitch��5d      OrientedEdgeSwitch��#��[ 5f      $��5 5 5 5q�5g      .��5h      �� $!��Resources/oriented_edge.png��6   $ HighQuality=�6=   5i      d� 5j      -   5        $ DeleteSwitch��5k      DeleteSwitch��#��[ 5l      $��5 5 5 5q�5n      .��5o      �� $��Resources/delete.png��6   $ HighQuality=�66   5p      5q      �� 5r   
   $��1��6   $��1��6   $��2��6.   $��5 5 5 5q�5s      
�������$��1q�5u       �� CornerRadius$ 10��5v      ._�5w      �� -   5         ��Name$ 
MainCanvas��5z      }�.  J� IsManipulationEnabled . ��5{      L� 5|       j� Viewport$ 	0,0,20,20�6#    j� ViewportUnits$ Absolute=�68    j� TileMode$
 Tile=�6Q    L� Drawing 5}      � 5~       � Brush
 ����� � Geometry 5   "   � 5�   &    � FillRule$ EvenOdd=�64   .��5�   *   ��  �� Rect$ 	0,0,50,50�6<   �� 5�   *   $ 	0,0,49,49�6<   5�   '   5�      5�      5�      5�      �� 5�   
   $��0��6   $��1��6"   $��2��62   $��Center=�6F    �� Orientation$ 
Horizontal=�6a   .+�5�      o� .@�5�      m� $��Graph��6   $��Center=�6*   .@�5�      m� -   5        $��
Open graph��5�      63   m� 5�      $��Open ...��6   .@�5�      m� -   5        $��Open matrix��5�   #   68   m� 5�      -	   5        $��Open аdjacency list��5�   #   6@   5�      m� 5�      -
   5        $��
Save graph��5�      63   m� 5�      $��Save ...��6   .@�5�      m� -   5        $��Save matrix��5�   #   68   m� 5�      -   5        $��Save аdjacency list��5�   #   6@   5�      m� 5�      -   5        $��Generate graph��5�      6O   m� 5�      -   5        $��Clear graph��5�      64   5�      5�      �� 5�      -   5        $��chooseAlgorithm��5�   (   $��5 0 0 0q�6   $	��100��6?   6K   �� 5�      -   5        $
��play��5�   &   $��5 0 5 0q�6   #��[ 6?   5�      �� 5�      -   5        $
��back��5�   &   $��5 0 5 0q�6   ��5�      �� 5�      .R�5�      �� $��Resources/back.png��6    6>   5�      5�      �� 5�      -   5        $��forward��5�   &   $��5 0 5 0q�6   ��5�      �� 5�      .R�5�      �� $��Resources/forward.png��6    6A   5�      5�      �� 5�      -   5        $��toend��5�   &   $��5 0 5 0q�6   ��5�      �� 5�      .R�5�      �� $��Resources/toend.png��6    6?   5�      5�      �� 5�      -   5        $
��fast��5�   &   $��5 0 5 0q�6   #��[ 6C   5�      �� 5�      $��Center=�6   .��6/   Speed66   �� 5�      -   5        $��speedChanger��5�       �� Orientation$ 
Horizontal=�5�       � Minimum$ 0Q�5�       � Maximum$ 5Q�5�       � Value$ 0Q�5�      $	��100��5�      $��Center=�5�      5�      5�      �� 5�   
   $��1��6   $��2��6$   $��5 5 0 0q�64   $
��Auto=�6E   $
��Auto=�6h   .��5�      �� .+�5�      �� -   5          ��Name$  	codeStack��5�      1   +GraphAlgorithmsVisualizer.UI.VariablesStack  5�      -   5        !  Name$! variableStack��5�   '   5�      5�      �� 5�   
   -   5        $  buttonStack��5�      $��2��6*   $��2��67   $��5 5 5 5q�6G   6Z   5�      5�      