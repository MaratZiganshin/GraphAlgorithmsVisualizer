   M S B A M L   `   `   `  ����T  PGraphAlgorithmsVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  5      )    #GraphAlgorithmsVisualizer.UI.EdgeUI   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:GraphAlgorithmsVisualizer.UI     5      -   5          >�Name$
  THIS��5       � 	Resources 5	       �� 
TargetType �� Property �� Value �� ElementName �� Path �� Mode��%0  '��       �� 5
   
   " ��6   .�5      ��  � �� 6&   �� $
 THIS��$ WeightBoxWidth�$ TwoWay=�6m   �� 5       � �� 6'   �� $
 THIS��$ WeightBoxHeight�$ TwoWay=�6o   �� 5       � ��$
 Left=�64   6C   �� 5       � ��$	 Top=�62   6@   5      5      .��5      � .+�5   
   (   "GraphAlgorithmsVisualizer.UI.Arrow  -   5          Name$ arrow��5      	   Start	 5      �� $
 THIS��$ LinePoints.From�$ OneWay=�

   End
 5      �� $
 THIS��$ LinePoints.To�$ OneWay=�   ArrowHeadVisibility 5      �� $
 THIS��$ DirectionVisibility�6c     5   
   -   5        $ revertArrow��5      	 5      �� $
 THIS��$ LinePoints.To�$ OneWay=�
 5      �� $
 THIS��$ LinePoints.From�$ OneWay=� 5      �� $
 THIS��$ RevertedVisibility�6b   �� 5   
   -   5         ��Name$ textbox��5      
������� �� TextAlignment$ Center=�5      ��5      �� *TextTopLeftCorner+$
 THIS��y�5      �� $
 THIS��$ WeightVisibility�5   ?   5       5!      