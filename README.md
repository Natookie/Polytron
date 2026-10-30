# Polytron

### List and explain any bugs, unexpected behaviors, or issues you encounter during development

## 1. FormatException on number input
When I deleted all characters from a number field, the console spammed `FormatException during event processing`. Cause: `int.Parse("")` throws. Also the clamp was updating the visible text but not the `_text` string behind it, so they went out of sync. Changed to `TryParse`, blocked non-digit characters, and made the clamp update `_text` too.

## 2. Inventory currency never went back to 0
Rolled a X-value item (showed X), then a Y-value item (showed X+X+Y, not X+Y). The bug was in `Bind()`, it added `sellValue` every time it ran, and `Bind` runs on every refresh, which runs after every add. So old items got counted again each time. Fix: `Bind` only sets visuals now. Adding inventory/storage currency is recalculated from `InventoryManager`'s `AddItem()` function.

## 3. Warning for "all weights zero" never showed
I put a warning for "all weights are zero" inside `if(sum <= 0)`. It never printed. Reason: after adding default weight system to pair, i forgot that having `if(defaultWeight > 0) return defaultWeight;` running first, it will blocked all text field content value. Fix: Should only use defaultWeight if text field parsing failed (NaN).

![Gacha roll demo](Assets/_Projects/2.%20Arts/gifpolytron.gif)