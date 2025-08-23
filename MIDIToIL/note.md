# Note

## TODO
* fix output to match IL spec.

## 言語仕様

### IL

```ebnf
program = {line}
line = chord, "\t|", melody, "\n"
chord = note, note, {note}
melody = {note}
note = note name, octave
note name = "A" | "B" | "C" | "D" | "E" | "F" | "G", ["#" | "b"]
octave = digit, {digit}
digit =  "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
```

## Note
* cakewalk Sonarでコード名を付けても、保存されなかった。
  * 歌詞にしたら保存された。なぜか別トラックになったが。