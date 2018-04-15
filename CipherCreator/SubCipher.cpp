// SubCipher.cpp
// Ian Cordova
// Senior Project - CipherBreaker

#include "SubCipher.h"
#include <random>
#include <ctime>

/*
SubCipher::SubCipher()

NAME
SubCipher::SubCipher - Default Constructor

SYNOPSIS
No passed variables

DESCRIPTION
Initializes the member m_Alphabet to contain the alphabet in capital letters.

RETURNS
None

AUTHOR
Ian Cordova

DATE LAST UPDATED
4:30pm 4/2/2018
*/
SubCipher::SubCipher() {

	m_Alphabet = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
		'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

	srand(time(NULL));
}

/*
map<char, char> SubCipher::SubCipher()

NAME
SubCipher::CreateSubCipher - Creates a random substitution cipher.

SYNOPSIS
No passed variables

DESCRIPTION
This method creates a random substitution cipher by creating a map with each key as a letter
and assigns each letter a random letter that it will be mapped to.

RETURNS
map<char, char> subCipher - contains the substitution cipher values.

AUTHOR
Ian Cordova

DATE LAST UPDATED
4:30pm 4/2/2018
*/
map<char, char> SubCipher::CreateSubCipher() {
	map<char, char> subCipher;
	int randLetter;
	
	for (int i = 0; i < 25; ++i) {
		do randLetter = rand() % 26; while ((CipherContains(subCipher, m_Alphabet[randLetter])));
		subCipher[m_Alphabet[i]] = m_Alphabet[randLetter];
		cout << m_Alphabet[i] << " : " << m_Alphabet[randLetter] << " " << endl;
	}
	return subCipher;
}

/*
bool SubCipher::CipherContains()

NAME
SubCipher::CipherContains()

SYNOPSIS
map<char, char> a_cipher - map of letters and their ciphered value (haystack)
char a_value - value to find in map (needle)

DESCRIPTION
Checks if char a_value already exists in a_cipher.

RETURNS
bool - true if value exists in map, otherwise false

AUTHOR
Ian Cordova

DATE LAST UPDATED
6:20pm - 4/15/2018
*/
bool SubCipher::CipherContains(map<char, char> a_cipher, char a_value) {

	for (auto const& i : a_cipher) {
		if (i.second == a_value) {
			return true;
		}
	}
	return false;
}

/*
SubCipher::~SubCipher()

NAME
SubCipher::~SubCipher - Destructor

SYNOPSIS
No passed variables

DESCRIPTION
Deletes the member m_Alphabet to contain the alphabet in capital letters.

RETURNS
None

AUTHOR
Ian Cordova

DATE LAST UPDATED
4:30pm 4/2/2018
*/
SubCipher::~SubCipher() {
	delete m_Alphabet;
}