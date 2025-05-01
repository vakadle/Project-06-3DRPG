// declare all your variables
VAR door1Clue = ""
VAR door1Clue_correct = false

VAR door2Clue = ""
VAR door2Clue_correct = false

VAR door3Clue = ""
VAR door3Clue_correct = false

VAR door4Clue = ""
VAR door4Clue_correct = false

VAR door5Clue = ""
VAR door5Clue_correct = false

VAR door6Clue = ""
VAR door6Clue_correct = false


=== door1 ===
Is there a {door1Clue} at my house?
+ "Yes — that's my house." -> door1_yes
+ "No — not mine."        -> door1_no

=== door1_yes ===
{ door1Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door1_no ===
{ !door1Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END


=== door2 ===
Is there a {door2Clue} at my house?
+ "Yes — that's my house." -> door2_yes
+ "No — not mine."        -> door2_no

=== door2_yes ===
{ door2Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door2_no ===
{ !door2Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END


=== door3 ===
Is there a {door3Clue} at my house?
+ "Yes — that's my house." -> door3_yes
+ "No — not mine."        -> door3_no

=== door3_yes ===
{ door3Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door3_no ===
{ !door3Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END


=== door4 ===
Is there a {door4Clue} at my house?
+ "Yes — that's my house." -> door4_yes
+ "No — not mine."        -> door4_no

=== door4_yes ===
{ door4Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door4_no ===
{ !door4Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END


=== door5 ===
Is there a {door5Clue} at my house?
+ "Yes — that's my house." -> door5_yes
+ "No — not mine."        -> door5_no

=== door5_yes ===
{ door5Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door5_no ===
{ !door5Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END


=== door6 ===
Is there a {door6Clue} at my house?
+ "Yes — that's my house." -> door6_yes
+ "No — not mine."        -> door6_no

=== door6_yes ===
{ door6Clue_correct ? "That's right! You found me." : "No, that's wrong. I don't live here." }
-> END

=== door6_no ===
{ !door6Clue_correct ? "That's right—I don't live here." : "No, that's wrong. I live here." }
-> END