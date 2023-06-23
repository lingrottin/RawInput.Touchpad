# RawInput.Touchpad
\[[Checkout the original README](/README_old.md)\]  
This intends to read input on a Precision Touchpad(PTP) and map it to the whole screen. This is modified from [emoacht/RawInput.Touchpad](https://github.com/emoacht/RawInput.Touchpad). Unlike [AbsoluteTouchEx](https://github.com/apsun/AbsoluteTouchEx), this won't inject into process. 

## Requirements
* .NET 7

## FAQ
(note: I don't speak English natively)
### Where to download?
Head to [Releases](https://github.com/lingrottin/RawInput.Touchpad/releases). You may need [.NET 7 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) installed.

### Will this trigger anti-cheating systems (such as osu!'s) ?
This program sets cursor position globally, not injecting into any program, which may cause delay. But it will greatly reduce the probability of being anti-cheated.

### It doesn't work in osu!
Please disable `Raw Input`.

### How to set the correct mapping?
Mapping format is like
```
a,b|c,d|e,f|g,h
```
where a, b, c, d, e, and f are all integer numbers.(or the program will crash!)  
You will need to set `a,b` as the position of the upper-left corner of the touchpad area where you intend to put your finger, and set `c,d` to the lower-right corner. `e,f` and `g,h` is similar, which is on-screen positions.

## Roadmap
- [x] Handle touchpad event \(done in the oringinal repo by [@emoacht](https://github.com/emoacht)\)
- [x] Handle touchpad event in background
- [x] Set mouse position
- [x] Establish the correct mapping between the screen and touchpad
- [ ] More easy-to-use
- [ ] Run in background
