   M S B A M L   `   `   `  ���� T  PGraphAlgorithmsVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  )    #GraphAlgorithmsVisualizer.UI.EdgeUI   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     <local*clr-namespace:GraphAlgorithmsVisualizer.UI     -     >�Name$
  THIS�� � 	Resources  �� 
TargetType �� Property �� Value �� ElementName �� Path �� Mode��%�   '��       �� " ��.���  � �� �� $
 THIS��$ WeightBoxWidth�$ TwoWay=���  � �� �� $
 THIS��$ WeightBoxHeight�$ TwoWay=���  � ��$
 Left=���  � ��$	 Top=�.��� .+�(   "GraphAlgorithmsVisualizer.UI.Arrow  -     Name$ arrow��	   Start	 �� $
 THIS��$ LinePoints.From�$ OneWay=�

   End
 �� $
 THIS��$ LinePoints.To�$ OneWay=�   ArrowHeadVisibility �� $
 THIS��$ DirectionVisibility�  -   $ revertArrow��	 �� $
 THIS��$ LinePoints.To�$ OneWay=�
 �� $
 THIS��$ LinePoints.From�$ OneWay=� �� $
 THIS��$ RevertedVisibility��� -    ��Name$ textbox��
������� �� TextAlignment$ Center=����� *TextTopLeftCorner+$
 THIS��y��� $
 THIS��$ WeightVisibility�