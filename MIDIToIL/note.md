# Note

## TODO

* fix output to match IL spec.
* make converter which convert IL to MAsm (Music Assembly).

## 言語仕様

### IL

```ebnf
program = {line};
line = chord, "\t|", melody, "\n";
chord = note, note, {note};
melody = {note};
note = note name, octave;
note name = "A" | "B" | "C" | "D" | "E" | "F" | "G", ["#" | "b"];
octave = digit, {digit};
digit =  "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";
```

### MAsm

* スタックを作って、値を積む操作と、スタックから引数の数分取って新しい値を返す関数を組み合わせよう。逆ポーランド記法的な。てかまあスタックマシンってやつか。

#### 構造

```ebnf
program = { "\n" | "\t" }, { { "\t" }, line , { "\n" } };
line = "discard"
	| push
	| "set"
	| "||:"
	| ":||"
	| "+"
	| "-"
	| "*"
	| "/"
	| "convert"
	| "input"
	| "output"
	| "comment"
	;
push = "push", value;
value = variable symbol | type symbol | int | float | string | bool;
variable symbol = "var", symbol;
type symbol = "type", symbol;
```

|命令		|stackからの入力			|出力		|コメント					|
|-			|-							|-			|-							|
|`discard`	|`value`					|			|stackの値を破棄する。		|
|`push`		|							|`value`	|文中のリテラル値を解釈して、stackに積む。|
|`set`		|`value, variable symbol`	|`value`	|変数に代入する。|
|`||:`		|`variable symbol`			|			|反復の開始マーカー。反復は、変数の値がtrueである間続く。|
|`:||`		|							|			|反復の終了マーカー。|
|`+`		|`value, value`				|`value`	|変数の値のtypeによる。|
|`-`		|`value, value`				|`value`	|変数の値のtypeによる。|
|`*`		|`value, value`				|`value`	|変数の値のtypeによる。|
|`/`		|`value, value`				|`value`	|変数の値のtypeによる。|
|`convert`	|`value, type symbol`		|`value`	|値の型を変換する。|
|`input`	|							|`string`	|標準入力。|
|`output`	|`value`					|			|標準出力。|
|`comment`	|							|			|コメント。|
* stackから入力した場合、それはstackから削除される。

#### ILからの変換

* 基本的に、chordが命令、melodyが文中の引数に対応する。
* 数値は12進数あるいは7進数、聞こえが悪かったら5音音階にして5進数に対応させる手もある。
* その場合、小数点をどう表現するのかという問題もあるが、実は整数を小数に変換してから、好きな分割れば良いんじゃないかという噂もある。
* boolも実は別にそれで良い。
* ただ、stringは複数の数値が必要なため、セパレータは何らかの形で必要。単純に数字を1つ減らせば良さそうではある。単にnull文字を区切りにすれば良いのか？

## Note

* cakewalk Sonarでコード名を付けても、保存されなかった。
  * 歌詞にしたら保存された。なぜか別トラックになったが。
