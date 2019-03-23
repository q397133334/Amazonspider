function Price(x, w) {
    var p = x;
    var yunfei = 65;
    if (w > 1) {
        yunfei += 35 * Math.ceil(w) - 35;
    }
    p = p * 6.9 + yunfei;
    return p;
}