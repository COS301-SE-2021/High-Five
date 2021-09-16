package com.bdpsolutions.highfive.utils
import org.apache.commons.codec.binary.Base64
import java.util.regex.Pattern

/**
 * Helper object to fetch user data from the JWT token returned by Azure.
 *
 * NOTE: There is a library to decode JWT tokens: https://github.com/auth0/JWTDecode.Android
 * but it conflicts with other dependencies in the project, hence why this class is used.
 */
object JWTDecoder {
    fun getFirstName(token: String) : String? {
        val body = getBody(token)
        val nameMatcher = Pattern.compile("\"given_name\" *: *\"(\\w+)\"").matcher(body)

        return if (nameMatcher.find()) {
            nameMatcher.group(1)
        } else null
    }

    private fun getBody(token: String): String {
        val splitString: List<String> = token.split(".")
        val base64EncodedBody = splitString[1]
        return String(android.util.Base64.decode(base64EncodedBody, android.util.Base64.DEFAULT))
    }
}