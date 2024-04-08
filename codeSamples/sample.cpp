#include <msgpack.hpp>
#include <iostream>
#include <vector>

struct Person {
    std::string name;
    int age;
    std::vector<std::string> hobbies;
    
    // MessagePack serialization method
    MSGPACK_DEFINE(name, age, hobbies);
};