// SubCipher.h
// Ian Cordova
// Senior Project - CipherBreaker

#pragma once
#include <iostream>
#include <map>
using namespace std;

class SubCipher {

public:

	// Default constructor
	SubCipher();

	// Creates a Substitution Cipher alphabet
	map<char, char> CreateSubCipher();

	// Destructor
	~SubCipher();

private:

	char * m_Alphabet;

	static bool CipherContains(map<char, char>, char);
};
