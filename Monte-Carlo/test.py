from string import ascii_lowercase

def f(n):
    alphabet = '0123456789' + ascii_lowercase
    base35_word = ''
    while n > 0:
        remainder = n % 35
        base35_word += alphabet[remainder]
        n //= 35

    return base35_word[::-1] if base35_word else '0'


word = int('cat', 36)
print(f(word))

