   M S B A M L   `   `   `  ����   MyPaint    MyPaint.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Untitled - My Paint��$	��750��$
��1284�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � Icon$ resources/favicon.ico�� J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ ApplicationCommands.Copy��  -   $ ApplicationCommands.Cut��  -   $ ApplicationCommands.Paste��  -   $ ApplicationCommands.Undo��  -   $ ApplicationCommands.Redo��  -   $ ApplicationCommands.Save��  -   $ ApplicationCommands.Open��  -	   $ ApplicationCommands.New��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $��30 � � ColumnDefinitions �� $��* �.+�� $��0��$��0��$
��Left=�.+�o� 
���   �$��24�� �� Left$ 25��
	 �� Top$	 14��$��	0,0,-62,0q���Z� .@�m� $
��File��.@�m� 
 m� Command$
 ApplicationCommands.New��$	��New�� m� Icon �� ����  �� Source$ resources/icons.png�� �� 
SourceRect$ 0,861,21,21��m� $
 ApplicationCommands.Open��$
��Open�� �� ���� $ resources/icons.png��$ 21,2838,21,21��m� $
 ApplicationCommands.Save��$
��Save�� �� ���� $ resources/icons.png��$ 21,1766,21,21��m� -
   $
��Exit�� �� ���� $ resources/icons.png��$ 21,882,21,21��m� $
��Edit��.@�m� $
 ApplicationCommands.Copy��$
��Copy�� �� ���� $ resources/icons.png��$ 0,2230,21,21��m� $
 ApplicationCommands.Cut��$	��Cut�� �� ���� $ resources/icons.png��$ 0,2648,21,21��m� $
 ApplicationCommands.Paste��$��Paste�� �� ���� $ resources/icons.png��$ 0,525,21,21��m� $��Delete�� �� ���� $ resources/icons.png��$ 21,819,21,21��m� $
 ApplicationCommands.Undo��$
��Undo�� �� ���� $ resources/icons.png��$ 0,1893,21,21��m� $
 ApplicationCommands.Redo��$
��Redo�� �� ���� $ resources/icons.png��$ 21,2229,21,21��m� $��Format��.@�m� $��
Background�� �� ���� $ resources/icons.png��$ 21,1110,21,21��.@�m� -   $��mnItmTransparent�� m� IsCheckable . $��Transparent��m� -   $��	mnItmFill��$
��Fill��m� $��Horizontal Alignment�� �� ���� $ resources/icons.png��$ 
0,84,21,21��.@�m� -   $
��Left��m� -   $��Right��m� -   $��Center��m� -   $��Justify��m� $��Vertical Alignment�� �� ���� $ resources/icons.png��$ 0,3407,21,21��.@�m� -   $	��Top��m� -   $��Bottom��m� -   $��Center��m� $
��Text�� �� ���� $ resources/icons.png��$ 0,420,21,21��.@�m� -   $��Remove Background��m� -   $��Remove Border��m� $
��Help��.@�m� $��Guide�� �� ���� $ resources/icons.png��$ 0,671,21,21��m� $
��Info�� �� ���� $ resources/icons.png��$ 21,2292,21,21��]� $��1��$��0��$��0q�$��Stretch=����	#FFEAEAEA$��50��$
��Auto��.+���  �� Orientation$ 
Horizontal=�$��Right=�.+��� $��ApplicationCommands.New��$��14, 2 ,2 ,2q�$��27��$��27�� � ToolTip$	 New��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,861,21,21���� $��ApplicationCommands.Open��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Open��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,2838,21,21���� $��ApplicationCommands.Save��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Save��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,1766,21,21���� $��7��$	 220��$��0q�$	 66��$��20�� J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~���  �� ScaleX$ -1Q��� ��  �� Angle$	 -90Q�T� d� -   $��tglbtnSelect��$��
2, 2 ,2 ,2q�$��27��$��27��$ Select��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,2838,21,21���� $��ApplicationCommands.Copy��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Copy��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,2230,21,21���� $��ApplicationCommands.Cut��$��
2, 2 ,2 ,2q�$��27��$��27��$	 Cut��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,2648,21,21���� $��ApplicationCommands.Paste��$��
2, 2 ,2 ,2q�$��27��$��27��$ Paste��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,525,21,21���� -   $��
2, 2 ,2 ,2q�$��27��$��27��$ Delete��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,819,21,21���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� �� $��ApplicationCommands.Undo��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Undo��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,1893,21,21���� $��ApplicationCommands.Redo��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Redo��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,2229,21,21���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� d� -   $��
tglbtnLine��$��
2, 2 ,2 ,2q�$��27��$��27��$
 Line��$��1q� d� 	IsChecked$
 TrueY���Z� .���� ���� $ resources/icons.png��$ 21,1914,21,21��d� -   $��tglbtnRectangle��$��
2, 2 ,2 ,2q�$��27��$��27��$ 	Rectangle��$��1q���Z� .���� ���� $ resources/shapes.png��$ 1,133,21,21��d� -   $��tglbtnEllipse��$��
2, 2 ,2 ,2q�$��27��$��27��$ Ellipse��$��1q���Z� .���� ���� $ resources/shapes.png��$ 1 ,177,21,21���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� �� $��
2, 2 ,2 ,2q�$��27��$��27��$
 Size������ $ resources/icons.png��$ 0,3261,21,21���� -   $��cbStrokeThickness�� �� SelectedIndex$ 2��$��
2, 2 ,2 ,2q�$��50��$��27��$
 Size��$
��2, 4q���Z� .@��� $
��2, 4q�.��1px�� $
��2, 4q�.��2px�� $
��2, 4q�.��3px�� $
��2, 4q�.��4px�� $
��2, 4q�.��8px�� $
��2, 4q�.��12px�� $
��2, 4q�.��16px�� $
��2, 4q�.��24px�� $��
2, 2 ,2 ,2q�$��27��$��27��$ Style������ $ resources/icons.png��$ 0,2859,21,21���� -   $��cbStrokeStyle��$ 0��$��
2, 2 ,2 ,2q�$��68��$��27��$ Style��$
��2, 5q���Z� .@��� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 	0,0,50,16���� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 
50,0,50,16���� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 100,0,50,16���� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 150,0,50,16���� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 200,0,50,16���� $
��2, 4q�.���� ���� $ resources/linestyle.png��$ 250,0,50,16���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� �� -   $��
2, 2 ,2 ,2q�$��27��$��27��$ Stroke��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,2145,21,21���� -   $��StrokeBrush��
���   �$	��0.5��
���   �$��
2, 2 ,2 ,2q�$��27��$��27��$ Stroke Color���� -   $��
2, 2 ,2 ,2q�$��27��$��27��$
 Fill��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,1110,21,21���� -    $��	FillBrush��
���   �$	��0.5��
�������$��
2, 2 ,2 ,2q�$��27��$��27��$ 
Fill Color���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� d� -!   $��tglbtnImage��$��
2, 2 ,2 ,2q�$��27��$��27��$ Image��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,3387,21,21���� $��7��$	 220��$��0q�$	 66��$��20��$ 0.5,0.5&�z�V� .~��� $ -1Q��� �� $	 -90Q�T� d� -"   $��
tglbtnText��{�.  $��
2, 2 ,2 ,2q�$��27��$��27��$
 Text��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,420,21,21���� -#   $��FontList��   System.Windows.Media.Fonts   SystemFontFamilies#��Z $ 1�� �� Text$
 Font��$��
2, 2 ,2 ,2q�$	��150��$��27��$ Font Family��$��5q���Z� ���� .R��� ���� ���� �� -$   $��txtFontSize��$��
2, 2 ,2 ,2q�$��Center=�$��37��$��27��$ 	Font size��$
��2, 4q���Z� .��12�� $��
2, 2 ,2 ,2q�$��27�� �� Padding$
 2, 4q���Z� px�� -%   $��
2, 2 ,2 ,2q�$��27��$��27��$ Stroke��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,2145,21,21���� -&   $��	TextColor��
���   �$	��0.5��
���   �$��
2, 2 ,2 ,2q�$��27��$��27��$ 
Text Color��d� -'   $��
tglbtnBold��$��
2, 2 ,2 ,2q�$��27��$��27��$��1q�$
 Bold����Z� .���� ���� $ resources/icons.png��$ 0,1787,21,21��d� -(   $��tglbtnItalic��$��
2, 2 ,2 ,2q�$��27��$��27��$ Italic��$��1q���Z� .���� ���� $ resources/icons.png��$ 21,1578,21,21��d� -)   $��tglbtnUnderline��$��
2, 2 ,2 ,2q�$��27��$��27��$ 	Underline��$��1q���Z� .���� ���� $ resources/icons.png��$ 0,399,21,21���� $��2��._��� -*   $��drawingCanvas��}�. ����  �� Color$ White���� $��3��$��30�����	#FFE4E4E4.@��� $��
Status bar��