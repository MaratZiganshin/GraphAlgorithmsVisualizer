   M S B A M L   `   `   `  ���� T  PGraphAlgorithmsVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  K*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  *    $GraphAlgorithmsVisualizer.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     <local*clr-namespace:GraphAlgorithmsVisualizer.UI     ?controls*clr-namespace:GraphAlgorithmsVisualizer.UI     V RMaterialDesignThemes.Wpf, Version=2.5.1.1297, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$!  Graph Algorithms Visualizer�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � 	Resources   Stop  
LeftButton �� 
TargetType  MaterialDesignRaisedButton �� BasedOn �� Property	 �� Value
 �� RelativeSource  
PlayButton �� 
TargetType  
StopButton  
FastButton 
	 Vertex *
 &MaterialDesignFloatingActionMiniButton�� %�  &	       &	      7  &	 �     &	 B    &	 �    &		 �    7 
 ��Resources/stop.png���� " d�8   .���  � ��
	 �������  � ��$	 5 5 5 5q���  � ��
	 �������  � ��	 �� *ActualHeight+
 �� *Self+��  � ��	 �� *ActualWidth+
 �� *Self+�� " ��
PlayButton��.���  � ��	 �� " ��.R��� $��Resources/play.png���� " ��
StopButton��.���  � ��	 �� " ��.R��� $��Resources/stop.png���� " ��
FastButton��.���  � ��	 �� " ��.R��� $��Resources/fast_play.png���� " ��8   .���  � ��
	 � �|���  � ��
	 �������  � ��	 �� *ActualHeight+
 �� *Self+��  � ��	 �� *ActualWidth+
 �� *Self+.���  � RowDefinitions �� $
��30px ��� �� $
��50px � � ColumnDefinitions �� $
��50px ��� �� $��500px �.+��� $��0��$��1��$��2��.+�d� -    d�Name$ CursorSwitch��CursorSwitch��#��[ $��5 5 5 5q�.���� $��Resources/cursor.png�� �� BitmapScalingMode$ HighQuality=�d� -   $ VertexSwitch��VertexSwitch��#��[ $��5 5 5 5q�.���� $��Resources/vertex.png��$ HighQuality=�d� -   $ 
EdgeSwitch��
EdgeSwitch��#��[ $��5 5 5 5q�.���� $��Resources/edge.png��$ HighQuality=�d� -   $ OrientedEdgeSwitch��OrientedEdgeSwitch��#��[ $��5 5 5 5q�.���� $!��Resources/oriented_edge.png��$ HighQuality=�d� -   $ DeleteSwitch��DeleteSwitch��#��[ $��5 5 5 5q�.���� $��Resources/delete.png��$ HighQuality=��� $��1��$��1��$��2��$��5 5 5 5q�
�������$��1q� �� CornerRadius$ 10��._��� -    ��Name$ 
MainCanvas��}�.  J� IsManipulationEnabled . ��L�  j� Viewport$ 	0,0,20,20� j� ViewportUnits$ Absolute=� j� TileMode$
 Tile=� L� Drawing �  � Brush
 ����� � Geometry �  � FillRule$ EvenOdd=�.����  �� Rect$ 	0,0,50,50��� $ 	0,0,49,49��� $��0��$��1��$��2��$��Center=� �� Orientation$ 
Horizontal=�.+�o� .@�m� $��Graph��$��Center=�.@�m� -   $��
Open graph��m� $��Open ...��.@�m� -   $��Open matrix��m� -	   $��Open аdjacency list��m� -
   $��
Save graph��m� $��Save ...��.@�m� -   $��Save matrix��m� -   $��Save аdjacency list��m� -   $��Generate graph��m� -   $��Clear graph���� -   $��chooseAlgorithm��$��5 0 0 0q�$	��100���� -   $
��play��$��5 0 5 0q�#��[ �� -   $
��back��$��5 0 5 0q����� .R��� $��Resources/back.png���� -   $��forward��$��5 0 5 0q����� .R��� $��Resources/forward.png���� -   $��toend��$��5 0 5 0q����� .R��� $��Resources/toend.png���� -   $
��fast��$��5 0 5 0q�#��[ �� $��Center=�.��Speed�� -   $��speedChanger�� �� Orientation$ 
Horizontal=� � Minimum$ 0Q� � Maximum$ 5Q� � Value$ 0Q�$	��100��$��Center=��� $��1��$��2��$��5 5 0 0q�$
��Auto=�$
��Auto=�.���� .+��� -     ��Name$  	codeStack��1   +GraphAlgorithmsVisualizer.UI.VariablesStack  -   !  Name$! variableStack���� -   $  buttonStack��$��2��$��2��$��5 5 5 5q�