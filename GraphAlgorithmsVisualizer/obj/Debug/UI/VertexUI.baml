   M S B A M L   `   `   `  ����T  PGraphAlgorithmsVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:GraphAlgorithmsVisualizer.UIGraphAlgorithmsVisualizer.UI  5      +    %GraphAlgorithmsVisualizer.UI.VertexUI   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:GraphAlgorithmsVisualizer.UI     5      -   5        6   0GraphAlgorithmsVisualizer.UI.PositionableControl   Name$
  THIS��5       �� Left 5	      ��  �� ElementName$
 THIS�� �� Path$ 
LeftOffset� �� Mode$ TwoWay=�
 �� Top 5
      �� $
 THIS��$ 	TopOffset�$ TwoWay=���5      �� $
 THIS��$ Diameter���5      �� $
 THIS��$ Diameter�.��5      � -   5         �Name$ 	container��5      .+�5   
   9   3GraphAlgorithmsVisualizer.UI.VertexDescriptionStack  -   5          Name$ descriptions��5   '   �� 5   
   -   5         ��Name$ ellipse��5      $
��1.25q�5      
���   �	 �� CornerRadius	 5      �� $
 THIS��$ Radius���5      �� $
 THIS��$
 Fill�._�5      �� -   5        
 ��Name$
 textDescription��5      $��20(�5      
�������$��Center=�5       �� TextAlignment$ Center=�5      $��Center=�5      ��5      �� $
 THIS��$
 Text�5   5   5      5      5      